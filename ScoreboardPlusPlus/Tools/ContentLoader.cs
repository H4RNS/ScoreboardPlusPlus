using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ScoreboardPlusPlus.Tools
{
    public static class ContentLoader
    {
        private static AssetBundle _bundle;

        private static readonly List<Content> _content = [];

        public static T LoadContent<T>(string contentId, string contentName) where T : Object
        {
            if (_content.Any(content => content.Id == contentId))
            {
                LogSource.LogWarning($"Content ID '{contentId}' already exists.");
                return GetContent<T>(contentId);
            }

            if (_bundle == null)
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Constants.BundlePath);
                _bundle = AssetBundle.LoadFromStream(stream);
            }

            T content = _bundle.LoadAsset<T>(contentName);

            if (content == null)
            {
                LogSource.LogWarning($"Content Name '{contentName}' isnt found.");
                return null;
            }

            if (content is GameObject gameObject)
            {
                content = Object.Instantiate(gameObject) as T;
            }

            _content.Add(new Content
            {
                Id = contentId,
                Asset = content
            });

            LogSource.LogInfo($"Content Name '{contentName}' of type {typeof(T).Name} Loaded");

            return content;
        }

        public static T GetContent<T>(string contentId) where T : Object => GetContent(contentId).Asset as T;

        private static Content GetContent(string contentId) => _content.FirstOrDefault(content => content.Id == contentId);

        private class Content
        {
            public string Id;
            public Object Asset;
        }
    }
}