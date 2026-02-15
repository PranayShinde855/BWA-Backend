using Microsoft.Extensions.Primitives;

namespace BWA.APIInfrastructure.Middlewares
{
    public class CSPMiddleware
    {
        private const string HEADER = "Content-Security-Policy";
        private readonly RequestDelegate _next;
        private readonly CspOptions _options;
        public CSPMiddleware(RequestDelegate next, CspOptions options)
        {
            this._next = next;
            this._options = options;
        }
        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add(HEADER, GetHeaderValue());
            context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("x-xss-protection", new StringValues("1; mode=block"));
            await this._next(context);
        }
        private string GetHeaderValue()
        {
            var value = "";
            value += GetDirective("default-src", this._options.Defaults);
            value += GetDirective("script-src", this._options.Scripts);
            value += GetDirective("style-src", this._options.Styles);
            value += GetDirective("img-src", this._options.Images);
            value += GetDirective("font-src", this._options.Fonts);
            value += GetDirective("media-src", this._options.Media);
            value += GetDirective("connect-src", this._options.Connect);

            return value;
        }
        private string GetDirective(string directive, List<string> sources) => sources.Count > 0 ? $"{directive} {string.Join(" ", sources)}; " : "";
    }
    public sealed class CspOptions
    {
        public List<string> Defaults { get; set; } = new List<string>();
        public List<string> Scripts { get; set; } = new List<string>();
        public List<string> Styles { get; set; } = new List<string>();
        public List<string> Images { get; set; } = new List<string>();
        public List<string> Fonts { get; set; } = new List<string>();
        public List<string> Media { get; set; } = new List<string>();
        public List<string> Connect { get; set; } = new List<string>();
    }
    public sealed class CspOptionsBuilder
    {
        private readonly CspOptions options = new CspOptions();
        public CspDirectiveBuilder Defaults { get; set; } = new CspDirectiveBuilder();
        public CspDirectiveBuilder Scripts { get; set; } = new CspDirectiveBuilder();
        public CspDirectiveBuilder Styles { get; set; } = new CspDirectiveBuilder();
        public CspDirectiveBuilder Images { get; set; } = new CspDirectiveBuilder();
        public CspDirectiveBuilder Fonts { get; set; } = new CspDirectiveBuilder();
        public CspDirectiveBuilder Media { get; set; } = new CspDirectiveBuilder();
        public CspDirectiveBuilder Connect { get; set; } = new CspDirectiveBuilder();
        internal CspOptions Build()
        {
            this.options.Defaults = this.Defaults.Sources;
            this.options.Scripts = this.Scripts.Sources;
            this.options.Styles = this.Styles.Sources;
            this.options.Images = this.Images.Sources;
            this.options.Fonts = this.Fonts.Sources;
            this.options.Media = this.Media.Sources;
            this.options.Connect = this.Connect.Sources;
            return this.options;
        }
    }
    public sealed class CspDirectiveBuilder
    {
        internal CspDirectiveBuilder()
        { }
        internal List<string> Sources { get; set; } = new List<string>();
        public CspDirectiveBuilder AllowSelf() => Allow("'self'");
        public CspDirectiveBuilder AllowNone() => Allow("none");
        public CspDirectiveBuilder AllowAny() => Allow("*");
        public CspDirectiveBuilder AllowUnsafeInline() => Allow("'unsafe-inline'");
        public CspDirectiveBuilder AllowUnsafeEval() => Allow("'unsafe-eval'");
        public CspDirectiveBuilder AllowNonce() => Allow("'nonce'");
        public CspDirectiveBuilder AllowData() => Allow("data:");
        public CspDirectiveBuilder AllowBlob() => Allow("blob:");
        public CspDirectiveBuilder AllowHttps() => Allow("https://*");
        public CspDirectiveBuilder AllowHttp() => Allow("http://*");
        public CspDirectiveBuilder Allow(string source)
        {
            this.Sources.Add(source);
            return this;
        }
    }
}
