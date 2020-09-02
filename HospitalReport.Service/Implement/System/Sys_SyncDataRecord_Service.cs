using System;
using System.Collections.Generic;
using HospitalReport.Service.Common;
using HospitalReport.Models.DataBase;
using HospitalReport.Models.Common;
using HospitalReport.Common;
using System.Diagnostics;

namespace HospitalReport.Service.Implement.System
{
    public class Sys_SyncDataRecord_Service
    {
        private readonly IRepository<Sys_SyncDataRecord, int> _syncDataRecordRepository;
        private readonly IRepository<Sys_SyncDataRecordHistory, int> _syncDataRecordHistoryRepository;

        public Sys_SyncDataRecord_Service(IRepository<Sys_SyncDataRecord, int> syncDataRecordRepository, IRepository<Sys_SyncDataRecordHistory, int> syncDataRecordHistoryRepository)
        {
            _syncDataRecordRepository = syncDataRecordRepository;
            _syncDataRecordHistoryRepository = syncDataRecordHistoryRepository;
        }

        /// <summary>
        /// 数据处理（定时任务）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateViews"></param>
        /// <param name="syncItemName"></param>
        /// <returns></returns>
        public ReturnedDataResult ProcessingData(Func<InputModel<Sys_SyncDataRecord>,
            ReturnedDataResult> updateViews,
            string syncItemCode)
        {
            var ipstring = GetIPCommon.GetLocalIP();
            var startDateTime = DateTime.UtcNow;
            var syncDataRecord = _syncDataRecordRepository.Get(t => t.SyncItemCode == syncItemCode && t.Status == 1);
            if (syncDataRecord == null)
            {
                return new ReturnedDataResult()
                {
                    Message = "同步数据记录表中没有初始配置",
                    Status = ReturnedStatus.Error
                };
            }
            var syncDataRecordHistory = (Sys_SyncDataRecordHistory)syncDataRecord;
            try
            {
                //最大时间如果大于当前时间，取当前时间
                syncDataRecord.MaxTime = syncDataRecord.MaxTime > DateTime.UtcNow ? DateTime.UtcNow : syncDataRecord.MaxTime;
                var inputView = new InputModel<Sys_SyncDataRecord>()
                {
                    InputView = new Sys_SyncDataRecord()
                    {
                        MaxTime = syncDataRecord.MaxTime,
                        MinTime = syncDataRecord.MinTime,
                        ExcuteEndTime = syncDataRecord.ExcuteEndTime,
                        PageSize = syncDataRecord.PageSize
                    }
                };
                var returnedResult = updateViews(inputView);
                if (returnedResult.Status.Equals(ReturnedStatus.Error))
                {
                    syncDataRecord.ErrorMessage = returnedResult.Message;
                    // return UpdateSyncDataRecordView(syncDataRecord);
                }

                TimeSpan t3 = DateTime.UtcNow - startDateTime;
                var endDateTime = DateTime.UtcNow;
                syncDataRecord.ExcuteStartTime = startDateTime;
                syncDataRecord.ExcuteEndTime = endDateTime;
                syncDataRecord.CreationTime = DateTime.UtcNow;
                syncDataRecord.LastModificationTime = DateTime.UtcNow;
                syncDataRecord.ErrorMessage = ipstring;
                syncDataRecord.MinTime = syncDataRecord.MaxTime;
                var maxTime = syncDataRecord.MaxTime.AddMinutes(syncDataRecord.TimeInterval);
                syncDataRecord.MaxTime = maxTime > DateTime.UtcNow ? DateTime.UtcNow : maxTime;
                syncDataRecordHistory.CreationTime = DateTime.UtcNow;
                syncDataRecordHistory.LastModificationTime = DateTime.UtcNow;
                _syncDataRecordHistoryRepository.Insert(syncDataRecordHistory);
                _syncDataRecordRepository.Update(syncDataRecord);
            }
            catch (Exception ex)
            {
                syncDataRecord.ErrorMessage = ex.Message;
                //   return UpdateSyncDataRecordView(syncDataRecord);
            }
            return new ReturnedDataResult()
            {
                Status = ReturnedStatus.Success,
                Message = "同步信息成功"
            };
        }

        /// <summary>
        /// 同步数据接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchViews"></param>
        /// <param name="syncItemName"></param>
        /// <returns></returns>
        public ReturnedDataResult SyncData<T>(Func<QueryViewModel<Sys_SyncDataRecord>,
            ReturnedDataResult<List<T>>> searchViews,
            string syncItemCode)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var startDateTime = DateTime.UtcNow;
            var totalCount = 0;
            //假定最小时间不可以往后追加
            var isMixTimeStop = false;

            var syncDataRecord = _syncDataRecordRepository.Get(t => t.SyncItemCode == syncItemCode && t.Status == 1);
            if (syncDataRecord == null)
                return new ReturnedDataResult
                {
                    Message = "同步数据记录表中没有初始配置或者状态已停用",
                    Status = ReturnedStatus.Error
                };
            var restApiUrl = syncDataRecord.ApiUrl;
            syncDataRecord.ExcuteStartTime = startDateTime;
            var syncDataRecordHistory = (Sys_SyncDataRecordHistory)syncDataRecord;
            //最大时间如果大于当前时间，取当前时间
            syncDataRecord.MaxTime =
                syncDataRecord.MaxTime > DateTime.UtcNow ? DateTime.UtcNow : syncDataRecord.MaxTime;
            var queryViewModel = new QueryViewModel<Sys_SyncDataRecord>
            {
                InputView = new Sys_SyncDataRecord
                {
                    MinTime = syncDataRecord.MinTime.AddSeconds(syncDataRecord.TimeReversal),
                    MaxTime = syncDataRecord.MaxTime,
                    IsPaging = syncDataRecord.IsPaging
                },
                PageSize = syncDataRecord.PageSize,
                Current = syncDataRecord.Current
            };
            try
            {
                switch (syncDataRecord.IsPaging)
                {
                    case 0:
                        {
                            var inputModelView = searchViews(queryViewModel);
                            syncDataRecord.ErrorMessage = inputModelView.Message;
                            if (inputModelView.Status.Equals(ReturnedStatus.Error))
                            {
                                syncDataRecord.ErrorMessage = "没有取到数据";
                                // return UpdateSyncDataRecordView(syncDataRecord);
                            }
                            if (inputModelView.Data == null)
                            {
                                isMixTimeStop = true;
                                break;
                            }
                            //inputModelView: 请求的DATA
                            var postreResult = HttpResponse.PostDataJson<ReturnedDataResult>(restApiUrl, "");
                            if (postreResult.Status.Equals(ReturnedStatus.Error))
                            {
                                syncDataRecord.ErrorMessage = postreResult.Message;
                                // return UpdateSyncDataRecordView(syncDataRecord);
                            }
                            totalCount = inputModelView.Data.Count;
                        }
                        break;

                    case 1:
                        {
                            var pageSize = syncDataRecord.PageSize;
                            queryViewModel.PageSize = pageSize;
                            queryViewModel.Current = 1;
                            var inputModelView = searchViews(queryViewModel);
                            syncDataRecord.ErrorMessage = inputModelView.Message;
                            if (inputModelView.Status.Equals(ReturnedStatus.Error))
                            {
                                syncDataRecord.ErrorMessage = "没有取到数据";
                                // return UpdateSyncDataRecordView(syncDataRecord);
                            }
                            if (inputModelView.Data == null || inputModelView.Data.Count == 0)
                            {
                                isMixTimeStop = true;
                                break;
                            }
                            //inputModelView: 请求的DATA
                            var postreResult = HttpResponse.PostDataJson<ReturnedDataResult>(restApiUrl, "");
                            if (postreResult.Status.Equals(ReturnedStatus.Error))
                            {
                                syncDataRecord.ErrorMessage = postreResult.Message;
                                // return UpdateSyncDataRecordView(syncDataRecord);
                            }
                            totalCount = inputModelView.Data.Count;
                            var pageCount = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize)) + 1;
                            for (var pageIndex = 2; pageIndex < pageCount; pageIndex++)
                            {
                                queryViewModel.Current = pageIndex;
                                inputModelView = searchViews(queryViewModel);
                                syncDataRecord.ErrorMessage = inputModelView.Message;
                                //if (inputModelView.Status.Equals(ReturnedStatus.Error))
                                //    return UpdateSyncDataRecordView(syncDataRecord);
                                postreResult = HttpResponse.PostDataJson<ReturnedDataResult>(restApiUrl, "");
                                //if (postreResult.Status.Equals(ReturnedStatus.Error))
                                //    return UpdateSyncDataRecordView(syncDataRecord);
                            }
                        }
                        break;
                }
                //同步成功
                //1.将原记录拷贝到历史同步表中
                //2.更新同步记录表（状态成功，时间区间，同步耗时，同步(总|成功|失败|)记录数）
                stopwatch.Stop();
                var endDateTime = DateTime.UtcNow;
                syncDataRecord.ExcuteStartTime = startDateTime;
                syncDataRecord.ExcuteEndTime = endDateTime;
                syncDataRecord.ExcuteDuration = stopwatch.Elapsed.Seconds;
                syncDataRecord.ErrorMessage = string.Empty;
                syncDataRecord.CreationTime = DateTime.UtcNow;
                syncDataRecord.LastModificationTime = DateTime.UtcNow;
                syncDataRecordHistory.CreationTime = DateTime.UtcNow;
                syncDataRecordHistory.LastModificationTime = DateTime.UtcNow;
                _syncDataRecordHistoryRepository.Insert(syncDataRecordHistory);
                if (!isMixTimeStop)
                    syncDataRecord.MinTime = syncDataRecord.MaxTime;
                var maxTime = syncDataRecord.MaxTime.AddMinutes(syncDataRecord.TimeInterval);
                syncDataRecord.MaxTime = maxTime > DateTime.UtcNow ? DateTime.UtcNow : maxTime;
                _syncDataRecordRepository.Update(syncDataRecord);
            }
            catch (Exception ex)
            {
                syncDataRecord.ErrorMessage = ex.Message;
                // return UpdateSyncDataRecordView(syncDataRecord);
            }
            return new ReturnedDataResult
            {
                Status = ReturnedStatus.Success,
                Message = "同步信息成功"
            };
        }
    }
}