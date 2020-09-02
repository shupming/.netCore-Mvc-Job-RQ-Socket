using HospitalReport.Extention;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HospitalReport.Controllers.BaseCommon
{
    [CheckLogin]
    public abstract class BaseController : Controller
    {

    }
}