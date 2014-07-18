﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using podcastinatorService.DataObjects;
using podcastinatorService.Models;

namespace podcastinatorService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            Database.SetInitializer(new podcastinatorInitializer());
        }
    }

    public class podcastinatorInitializer : ClearDatabaseSchemaIfModelChanges<podcastinatorContext>
    {
        protected override void Seed(podcastinatorContext context)
        {
            List<PodCastItem> podcastItems = new List<PodCastItem>
            {
                new PodCastItem { Id = "1", Text = "First item", Complete = false },
                new PodCastItem { Id = "2", Text = "Second item", Complete = false },
            };

            foreach (PodCastItem todoItem in podcastItems)
            {
                context.Set<PodCastItem>().Add(todoItem);
            }

            base.Seed(context);
        }
    }
}

