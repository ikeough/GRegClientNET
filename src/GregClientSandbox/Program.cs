﻿using System;
using System.Collections.Generic;
using Greg;
using Greg.Requests;
using Greg.Responses;
using Greg.Utility;
using Greg.AuthProviders;
using RestSharp;

namespace GregClientSandbox
{
    internal class Samples
    {
        /// <summary>
        /// A basic auth provider specifying the test user
        /// name and password that correspond to the test environment on Greg.
        /// </summary>
        private static BasicProvider provider = new BasicProvider("test", "e0jlZfJfKS");

        /// <summary>
        /// A GregClient specifying the local host. The alternate IP provided is for 
        /// production Reach. 
        /// </summary>
        //private static GregClient pmc = new GregClient(provider, "http://107.20.146.184/");
        private static GregClient pmc = new GregClient(provider, "http://localhost:8080/");

        private static string DownloadPackageByIdTest()
        {
            var nv = new PackageDownload("5225e7dde2f476ca05000057");
            var response = pmc.Execute(nv);
            var pathDl = PackageDownload.GetFileFromResponse(response);
            var output = FileUtilities.UnZip(pathDl);
            return pathDl;
        }

        private static void UploadDynamoPackageVersionTest()
        {
            var keywords = new List<string>() {"neat", "ok"};
            var nv = new PackageVersionUploadRequestBody("Third .NET Package", "2.1.0", "", keywords, "contents", "dynamo", "0.1.0", "metadata", "group",
                            new List<PackageDependency>() { new PackageDependency("peter", "0.1.0"), new PackageDependency("stephen", "0.1.0") }, "", "", 
                            false, new List<String>());

            var files = new List<string>() {"../test/pedro.dyf", "../test/RootNode.dyf"};
            var request = new PackageVersionUpload(nv, files);
            Console.WriteLine(request.RequestBody.AsJson());
        }

        private static void UploadDynamoPackageVersionWithFilesTest()
        {
            var keywords = new List<string>() { "neat", "ok" };
            var nv = new PackageVersionUploadRequestBody("Third .NET Package", "2.1.0", "", keywords, "contents", "dynamo", "0.1.0", "metadata", "group", 
                new List<PackageDependency>() { new PackageDependency("peter", "0.1.0"), new PackageDependency("stephen", "0.1.0") }, "", "", false, new List<String>() );

            var files = new List<string>() {"../../../../test/pedro.dyf", "../../../../test/RootNode.dyf"};

            var request = new PackageVersionUpload(nv, files);

            var rr = new RestRequest();
            request.Build(ref rr);

            //var response = pmc.ExecuteAndDeserialize(nv);
            Console.WriteLine(request.RequestBody.AsJson());
        }

        private static void DownloadDynamoPackageByEngineAndNameTest()
        {
            var nv = new HeaderDownload("dynamo", "Third .NET Package");
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<PackageHeader>(nv);
            Console.WriteLine(pkgResponse.content); // the package
        }

        private static void DownloadDynamoPackageByIdTest()
        {
            var nv = new HeaderDownload("51eccaac7fa5b0146b000005");
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<PackageHeader>(nv);
            //var pkgResponse = pmc.Execute(nv);
            
            Console.WriteLine(pkgResponse.content.name); // the package
        }

        private static void DownloadAllPackagesTest()
        {
            var nv = HeaderCollectionDownload.All();
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<List<PackageHeader>>(nv);
            Console.WriteLine(pkgResponse.content);
        }

        private static void DownloadByEngineTest()
        {
            var nv = HeaderCollectionDownload.ByEngine("dynamo");
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<List<PackageHeader>>(nv);
            Console.WriteLine(pkgResponse.content);
        }

        private static void SearchWithQueryTest()
        {
            var nv = new Search("*ython");
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<List<PackageHeader>>(nv);
            Console.WriteLine(pkgResponse.content);
        }
        
        private static void ValidateAuthTest()
        {
            var nv = new ValidateAuth();
            var pkgResponse = pmc.ExecuteAndDeserialize(nv);
            Console.WriteLine(pkgResponse.message);
        }

        private static void GetWhitelistTest()
        {
            var nv = WhitelistHeaderCollectionDownload.All();
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<List<PackageHeader>>(nv);

            Console.WriteLine(pkgResponse.message);
            Console.WriteLine(pkgResponse.content);
        }
        
        static void Main(string[] args)
        {
            //DownloadPackageByIdTest();
            //DownloadAllPackagesTest();
            GetWhitelistTest();
            Console.Read();
        }
    }
}

