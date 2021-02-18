﻿using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DependencyResolvers.Autofac
{
    // Bu projeyi ilgilendiren configurasyonu burada yaptığımız için business module adını verdik.
    public class AutofacBusinessModule : Module //Autofac Module'unden implement edildi. Startuptaki dependency injection ortamını kurmamızı sağlıyor.
    {
        protected override void Load(ContainerBuilder builder) // Uygulama hayata geçtiği zaman, bu kod bloğu çalışacak.
        {
            // Singleınstance => AddSıngleton  : bir instance üretir herkese onu verir.
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();  // Senden IProductService istendiğinden ProductManager'ı register et. (ProductManager instance'ı ver.)
            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();

            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();

            builder.RegisterType<OrderManager>().As<IOrderService>().SingleInstance();
            builder.RegisterType<EfOrderDal>().As<IOrderDal>().SingleInstance();

            builder.RegisterType<EmployeeManager>().As<IEmployeeService>().SingleInstance();
            builder.RegisterType<EfEmployeeDal>().As<IEmployeeDal>();

            builder.RegisterType<CustomerManager>().As<ICustomerService>().SingleInstance();
            builder.RegisterType<EfCustomerDal>().As<ICustomerDal>().SingleInstance();



            // Autof bize interceptor görevi veriyor.

            // Çalışan uygulamamız içerisinde, -kayıt etmiş olduğumuz sınıfların her biri için - implement edilmiş interfaceleri bulur, onlar için AspetInterceptorSelector'ı çağır.
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector() // bütün sınıflar için gidip bakıyor o sınıfın herhangi bir aspect'i var mı.
                }).SingleInstance();

        }
    }
}
