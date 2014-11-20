﻿using Microsoft.AspNet.Hosting;
using Microsoft.Framework.Runtime;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Singularity
{
    public sealed class FileSystemHelper
    {
        private IApplicationEnvironment _appEnv;
        private SingularityConfig _config;
        private IHostingEnvironment _hostingEnv;

        public FileSystemHelper(IApplicationEnvironment appEnv, IHostingEnvironment hostingEnv, SingularityConfig config)
        {
            _appEnv = appEnv;
            _config = config;
            _hostingEnv = hostingEnv;
        }

        /// <summary>
        /// Returns the cache folder for composite files for the current compression supported
        /// </summary>
        /// <returns></returns>
        public string GetCurrentCompositeFolder(CompressionType type)
        {
            return Path.Combine(CurrentCacheFolder, type.ToString());
        }

        public string GetCurrentCompositeFilePath(CompressionType type, string filesetKey)
        {
            return Path.Combine(GetCurrentCompositeFolder(type), filesetKey + ".s");
        }

        /// <summary>
        /// A rudimentary map path function
        /// </summary>
        /// <param name="contentFile"></param>
        /// <returns></returns>
        internal string MapPath(string contentFile)
        {
            return Path.Combine(WebRoot, contentFile).Replace("/", "\\");
        }

        internal async Task<string> ReadContentsAsync(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            using (var reader = new StreamReader(fileStream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        internal async Task WriteContentsAsync(string filePath, string contents)
        {
            using (var writer = File.CreateText(filePath))
            {
                await writer.WriteAsync(contents);
            }
        }

        /// <summary>
        /// The current cache folder for the current version
        /// </summary>
        /// <returns></returns>
        public string CurrentCacheFolder
        {
            get
            {
                return Path.Combine(_appEnv.ApplicationBasePath, _config.DataFolder, "Cache", _config.ServerName, _config.Version);
            }
        }

        /// <summary>
        /// Returns the web root
        /// </summary>
        public string WebRoot
        {
            get
            {
                return _hostingEnv.WebRoot;
            }
        }
    }
}