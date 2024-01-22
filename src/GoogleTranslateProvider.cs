using System;
using EPiServer.Labs.LanguageManager;
using EPiServer.Labs.LanguageManager.Business.Providers;
using EPiServer.Labs.LanguageManager.Configuration;
using EPiServer.Labs.LanguageManager.Models;
using Google.Cloud.Translation.V2;

namespace A2Z.Episerver.Labs.LanguageManager.GoogleTranslate
{
    public class GoogleTranslateProvider : IMachineTranslatorProvider
    {
        private TranslationClient TranslationClient;
        public string DisplayName => "Google Translate";

        public bool Initialize(ITranslatorProviderConfig config)
        {
            var languageManagerOptions = new LanguageManagerOptions();
            var languageManagerConfig = new LanguageManagerConfig(languageManagerOptions);
            var subscriptionKey = languageManagerConfig.ActiveTranslatorProvider.SubscriptionKey;

            TranslationClient = TranslationClient.CreateFromApiKey(subscriptionKey);
            return true;
        }

        public TranslateTextResult Translate(string inputText, string sourceLanguage, string targetLanguage)
        {
            var translateTextResult = new TranslateTextResult
            {
                IsSuccess = true,
                Text = ""
            };

            if (string.IsNullOrWhiteSpace(inputText))
            {
                return translateTextResult;
            }

            try
            {
                var response = TranslationClient.TranslateText(inputText, targetLanguage, sourceLanguage);
                translateTextResult.Text = response.TranslatedText;
                translateTextResult.IsSuccess = true;
            }
            catch (Exception ex)
            {
                translateTextResult.IsSuccess = false;
                translateTextResult.Text = "An unexpected error occurred: " + ex.Message;
            }

            return translateTextResult;
        }
    }
}