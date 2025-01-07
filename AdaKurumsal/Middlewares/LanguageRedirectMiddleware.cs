namespace AdaKurumsal.Middlewares
{
    public class LanguageRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var path = request.Path.Value;

            if (path == "/")
            {
                var userLanguages = context.Request.Headers["Accept-Language"].ToString();
                var defaultLanguage = "tr"; // Varsayılan dil, eğer tarayıcı dili bulunamazsa kullanılacak

                if (!string.IsNullOrEmpty(userLanguages))
                {
                    var languages = userLanguages.Split(',');
                    if (languages.Length > 0)
                    {
                        var userLanguage = languages[0].Split('-')[0]; // İlk dili al, örn: "en-US" -> "en"
                        if (userLanguage == "tr")
                        {
                            context.Response.Redirect("/tr");
                            return;
                        }
                        else
                        {
                            context.Response.Redirect("/en");
                            return;
                        }
                    }
                }
                context.Response.Redirect($"/{defaultLanguage}");
                return;
            }

            await _next(context);
        }
    }
}
