using System;
using System.Threading.Tasks;
using EPiServer.Labs.LanguageManager.Business.Providers;
using EPiServer.Labs.LanguageManager.Configuration;
using EPiServer.Labs.LanguageManager.Models;
using Google.Cloud.Translation.V2;

namespace A2Z.Episerver.Labs.LanguageManager.GoogleTranslate
{
    public class GoogleTranslateProvider(ILanguageManagerConfig languageManagerConfig) : IMachineTranslatorProvider
    {
        private TranslationClient _translationClient;

        public string DisplayName => "Google Translate";

        public bool Initialize(ITranslatorProviderConfig config)
        {
            _translationClient = TranslationClient.CreateFromApiKey(languageManagerConfig.ActiveTranslatorProvider.SubscriptionKey);
            return true;
        }

        public async Task<TranslateTextResult> TranslateAsync(string inputText, string sourceLanguage, string targetLanguage)
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
                var response = await _translationClient.TranslateTextAsync(inputText, targetLanguage, sourceLanguage);
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