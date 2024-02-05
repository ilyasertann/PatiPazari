public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    dynamic _token;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
        _token = null;
      }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Cookie", out var cookieHeader))
        {
            // Eğer Cookie header yoksa, oturum açma işlemi gerçekleştir
            await PerformLogin(context);
        }
        else
        {
            // Eğer Cookie header varsa, token'ı çıkar ve kontrol et
            var cookies = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(cookieHeader);

            if (cookies.ContainsKey("access_token"))
            {
                _token = cookies["access_token"];

                if (IsTokenValid(_token))
                {
                    // Token geçerliyse, devam et
                    context.Request.Headers["Authorization"] = "Bearer " + _token;
                    await _next(context);
                }
                else
                {
                    // Token geçersizse, oturum açma işlemi gerçekleştir
                    await PerformLogin(context);
                }
            }
            else
            {
                // access_token bulunamazsa veya geçerli değilse, oturum açma işlemi gerçekleştir
                await PerformLogin(context);
            }
        }
    }

    private async Task PerformLogin(HttpContext context)
    {
        // Oturum açma işlemini gerçekleştir
        // Bu kısmı LoginController'daki Post metodu gibi doldurabilirsiniz

        // Örneğin:
        // var loginResponse = await _httpClient.PostAsync("api/Login", new StringContent("username=yourUsername&password=yourPassword"));
        // var token = await loginResponse.Content.ReadAsStringAsync();

        // Token'ı HTTP Header'a ekleyin
        context.Request.Headers["Authorization"] = "Bearer " + _token;

        await _next(context);
    }

    private bool IsTokenValid(string token)
    {
        // Token'ın geçerliliğini kontrol et
        // Bu kısmı sizin token doğrulama mekanizmanıza göre doldurmalısınız

        // Örneğin:
        // return _tokenValidator.ValidateToken(token);
        return true;
    }
}
