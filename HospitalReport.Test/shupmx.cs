
namespace HospitalReport.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Reflection;
    using System.Security.Permissions;

    // Declare three delegate types for demonstrating the combinations
    // of static versus instance methods and open versus closed
    // delegates.
    //
    public delegate void D1(C c, string s);
    public delegate void D2(string s);
    public delegate void D3();

    // A sample class with an instance method and a static method.
    //
    public class C
    {
        private int id;
        public C(int id) { this.id = id; }

        public void M1(string s)
        {
            Console.WriteLine("Instance method M1 on C:  id = {0}, s = {1}",
                this.id, s);
        }

        public static void M2(string s)
        {
            Console.WriteLine("Static method M2 on C:  s = {0}", s);
        }
    }

    [TestClass]
    public class UnitTest1x
    {
        [TestMethod]
        public void TestMethod111()
        {


            C c1 = new C(42);

            // Get a MethodInfo for each method.
            //
            MethodInfo mi1 = typeof(C).GetMethod("M1",
                BindingFlags.Public | BindingFlags.Instance);
            MethodInfo mi2 = typeof(C).GetMethod("M2",
                BindingFlags.Public | BindingFlags.Static);

            D1 d1;
            D2 d2;
            D3 d3;


            Console.WriteLine("\nAn instance method closed over C.");
            // In this case, the delegate and the
            // method must have the same list of argument types; use
            // delegate type D2 with instance method M1.
            //
            Delegate test =
                Delegate.CreateDelegate(typeof(D2), c1, mi1, false);


            if (test != null)
            {
                d2 = (D2)test;

                // The same instance of C is used every time the 
                // delegate is invoked.
                d2("Hello, World!");
                d2("Hi, Mom!");
            }


            Console.WriteLine("\nAn open instance method.");
            // In this case, the delegate has one more 
            // argument than the instance method; this argument comes
            // at the beginning, and represents the hidden instance
            // argument of the instance method. Use delegate type D1
            // with instance method M1.
            //
            d1 = (D1)Delegate.CreateDelegate(typeof(D1), null, mi1);

            // An instance of C must be passed in each time the 
            // delegate is invoked.
            //
            d1(c1, "Hello, World!");
            d1(new C(5280), "Hi, Mom!");


            Console.WriteLine("\nAn open static method.");
            // In this case, the delegate and the method must 
            // have the same list of argument types; use delegate type
            // D2 with static method M2.
            //
            d2 = (D2)Delegate.CreateDelegate(typeof(D2), null, mi2);

            // No instances of C are involved, because this is a static
            // method. 
            //
            d2("Hello, World!");
            d2("Hi, Mom!");


            Console.WriteLine("\nA static method closed over the first argument (String).");
            d3 = (D3)Delegate.CreateDelegate(typeof(D3),
                "Hello, World!", mi2);
            d3();

        }

    }

}
