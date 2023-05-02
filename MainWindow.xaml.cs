using KitX.Contract.CSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ResourceMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IIdentityInterface
    {
        private readonly Controller controller;
        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(SB_Loaded);

            controller = new(this);

            Closed += (_, _) => Environment.Exit(0);
        }

        private void SB_Loaded(object sender, RoutedEventArgs e)
        {
            //设置定时器
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(10000000);   //时间间隔为1秒
            timer.Tick += new EventHandler(ch_tbCpuPercent);

            //开启定时器
            timer.Start();
        }
        PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        //PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        public void ch_tbCpuPercent(object sender, EventArgs e)
        {
            var cpuUsage = cpuCounter.NextValue();
            string cpuUsageStr = string.Format("{0:f2} %", cpuUsage);
            //var ramAvailable = ramCounter.NextValue();
            //string ramAvaiableStr = string.Format("{0} MB", ramAvailable);
            tbCpuPercent.Text = cpuUsageStr;
        }

        #region IIdentityInterface 接口
        /// <summary>
        /// 获取插件名称
        /// </summary>
        /// <returns>插件名称</returns>
        public string GetName() => "ResourseMonitor";

        /// <summary>
        /// 获取插件版本
        /// </summary>
        /// <returns>插件版本</returns>
        public string GetVersion() => "0.0.1";

        /// <summary>
        /// 获取显示名称
        /// </summary>
        /// <returns>显示名称</returns>
        public Dictionary<string, string> GetDisplayName() => new()
        {
            { "zh-cn", "资源监视器" },
            { "zh-cnt", "顯示名稱" },
            { "en-us", "Display Name" },
            { "ja-jp", "番組名" }
        };

        /// <summary>
        /// 获取作者名称
        /// </summary>
        /// <returns>作者名称</returns>
        public string GetAuthorName() => "StarInk";

        /// <summary>
        /// 获取发行者名称
        /// </summary>
        /// <returns>发行者名称</returns>
        public string GetPublisherName() => "船出";

        /// <summary>
        /// 获取作者链接
        /// </summary>
        /// <returns>作者链接</returns>
        public string GetAuthorLink() => "作者链接";

        /// <summary>
        /// 获取发行者链接
        /// </summary>
        /// <returns>发行者链接</returns>
        public string GetPublisherLink() => "发行者链接";

        /// <summary>
        /// 获取简单描述
        /// </summary>
        /// <returns>简单描述</returns>
        public Dictionary<string, string> GetSimpleDescription() => new()
        {
            { "zh-cn", "一个简单的资源监视器" },
            { "zh-cnt", "簡單描述" },
            { "en-us", "Simple Description" },
            { "ja-jp", "簡単な説明" }
        };

        /// <summary>
        /// 获取复杂描述
        /// </summary>
        /// <returns>复杂描述</returns>
        public Dictionary<string, string> GetComplexDescription() => new()
        {
            { "zh-cn", "一个只显示cpu占用率还敢号称资源监视器的资源监视器" },
            { "zh-cnt", "複雜描述" },
            { "en-us", "Complex Description" },
            { "ja-jp", "複雑な説明" }
        };

        /// <summary>
        /// 获取 MarkDown 语法的完整介绍
        /// </summary>
        /// <returns>完整介绍</returns>
        public Dictionary<string, string> GetTotalDescriptionInMarkdown() => new()
        {
            { "zh-cn", "完整描述" },
            { "zh-cnt", "完整描述" },
            { "en-us", "Total Description" },
            { "ja-jp", "完全な説明" }
        };

        /// <summary>
        /// 获取 Base64 编码的图标
        /// </summary>
        /// <returns>Base64 编码的图标</returns>
        public string GetIconInBase64() => "iVBORw0KGgoAAAANSUhEUgAAAMgAAADICAYAAACtWK6eAAAAAXNSR0IArs4c6QAAFJJJREFUeF7tnXuQHNV1h8/pGe1K6FE4kZHNy0RaB7IGsdPdyyLEY3gJEgw21gNMMA87MVBAME7hkBCDbVKQkMQEQ0xMcCUBBxMWAxHm4WBgbSzEavv2yBJsoKKCgCkT3l6t0OxqVvekLjWCRax2+va904+Z01X7155z7rm/e745PT3dfRH4YAVYgV0qgKwNK8AK7FoBBoSrgxWYRgEGhMuDFWBAuAZYgXgKcAeJpxt7tYkCDEibLDRPM54CDEg83dirTRRgQNpkoXma8RRgQOLpxl5togAD0iYLzdOMpwADEk839moTBRiQNllonmY8BRiQeLqxV5sowIC0yULzNOMpwIDE04292kSBTADiuu5iAJiPiLPUn5RyruM4mcitTeogC9OcIKIqAGx1HGfLxMTE611dXc/19/dvTzO5VIqwu7t7zuzZs7uIyCWiUwFgXwCYCQC/m6YYPHZmFKgi4jMA8BIRPeo4zpNDQ0MbAEAmnWGigNTBOFRKeR4AHAkAeyQ9YR4vlwq8BAC/QMTvzpkzZ3BgYGAiqVkkAki5XC5u2bLlcCL6UwBYBgAdSU2Qx2kpBd5GxMcR8e+HhoaeSqKjNB2Qvr6+ebVa7SJE/CYAFFtquXgyaSnwFhHdvGjRoqua/R2lqYAsXrx4j46OjiuJ6MK0lORxW1aB7Yj4/Tlz5lwyMDAw1qxZNg2Qgw8+eK9isfhtAFjVrOQ5LitARA91dnYuX7t2rboCZv1oCiCe580HgH9kOKyvFwecWoEHAOBUIUTNtkDWASmXyzM3b978TUT8mu1kOR4rMI0CNwghLgUAsqmSdUB83/8iEd0KANZj25w4x2o5BYiIzgrD8Ac2Z2a1iH3fP5CI7geA/WwmybFYgYgKvAkABwgh3oho39DMGiDd3d0ds2bN+jsAuLjhqGzACjRJAUS8JQiC822dalkDpFQqHek4zsMAMKtJc+ewrEAUBcYBoFcIsTGKcSMbK4AsXbp07tjY2O0A8JlGA/L/WYFmK4CIDwdBcJKNX9qtAOK67qGI+CAAfKTZk+f4rEAEBaSUcp9KpfLrCLbTmlgBxPO8CwDgu6bJsD8rYEsBRDwjCIIfmsazBUg/AKwwTYb9WQGLCtwBAOeY/nhoDIjneQcAgEqmZGFy/0tETyDiMCImfu+/hfw5REwFiGg2AHQBwNEA8PGYYSa7vb19+/aF69ev/41JLGNAXNc9ARG/BwCfMEkEAP65VqtdumHDhncAwCmXy87o6KhxfoY5sXsCCixcuFD29/erD0RSd39PTEzcZuOCj5RyL9PvIcYF6Pv+CiJSp1gmx4aRkZFDNm3apC7R8dHmCvT09OxeKBTUD86Hm0ghpfxUpVIZNolhA5AvEJEiPu6h7p25TghxedwA7Nd6Cvi+/w0iuspwZocKIQZNYhgD4rruhYh4k0ESWxHx/CAI1D00Vm80M8iJXVNWwPO8zwLAvSZpOI5z/NDQ0E9NYhgD4vv+RUR0o0ESm4no3DAM7zGIwa4tpoANQBBxWRAEj5hIkwVARuqAGH1amIjAvtlTgAF5f00YkOzVZ+oZMSAMSOpFmOUEGBAGJMv1mXpuDAgDknoRZjkBBoQByXJ9pp4bA8KApF6EWU6AAWFAslyfqefGgDAgqRdhlhNgQBiQLNdn6rkxIAxI6kWY5QQYEAYky/WZem4MCAOSehFmOQEGhAHJcn2mnhsDwoCkXoRZToABYUDeVcDzPPWCgjOI6EhEfPe5fiJ6ERF/rl6GIYR4JcuF3KzcGBAGRMHxLQD4eoMiu1oIcWWzCjGrcRmQNgfEdd3/RMRTohQoEa0Ow7CtXuvKgLQxIJ7n/RsAnBUFjkk2twkhztb0ya05A9KmgHietxIA7opZuauEEKavWIo5dLJuDEibAuK67gAiHhWn3IjoZ2EYluP45s2HAWlDQOpXrEzfOL5nO1zZYkDaE5BlAPATw0/zE4QQ/2UYI/PuDEh7AmLy/WOHYm3xPYQBYUDifoozIBGV4xfHRRQqK2aGV7C4g2guJAOiKVja5gxI9BXgUyw+xYpeLR+05FOsiMpxB4koVFbMuINEXwnuINxBolcLd5BYWnEHiSVbek7cQaJrzx0kQx2kr69vwbZt2357xowZ1XXr1r0QfRn1LBmQ6HoxICkD0tfXt3etVrsYEdX21QsnLd0WAHhQSnlLpVJ5NPqSNrZkQBprtMOCAUkRENd1v4yI3wGAzumWDBFvCYLgvOjLOr0lAxJdSQYkJUBc170MEa+LvlTwYyHEyRr2uzRlQKKr2DKAeJ53PgDcHH3qH7J8h4i+EIZh07dgc133RER8SDdXIro+DMOv6vrtbM+ARFfQ9/3lRHR3dI8pLY1v7DTeo9B13dMQ8U6DiVQR8fggCNYYxIjk6nneUwDQF8l4JyMpZU+lUvllHN9J59V8s2JEAUul0rGO4xjtUEtEfhiGIuKQU5oZA9Lb23uYlFI9IbdXzES2Sin3q1Qqr8f0j+RWKpWOdBznZ5GMpzCy0UW4g0RXv1QqHew4TggATnSvD1jKWq328Q0bNrwW0/9dNxuA7COlVI+BxvpkBoBfCSH2NZlEFF/Xda9CxG9Esd2FzUYhxGIDf/UWE+4gEQUslUp7Oo6zWr0ZKaLLzmbPzZ07t2dgYGAspr8dQFQUz/PUq2vUK2y0D0S8JgiCK7QdNR1837+diM7UdJtsPi6EmGngz4BoiFcul4ujo6OXA8DVGm7vmRLRFWEYXqteMxbHf4ePcQepA6I6gKL9YM1knh4ZGfE3bdo0rumnbe55njoNVJ/gsQ8hhJFe3EH0pFePKCPiavVdQs8TngGAQ4QQWzX9PmRutOCTo9WvEClIZkRMSl29Wh6GoekjqJGGY0AiyZQ5I9/3S0T0CwDYLWJyCopjhBCDEe2nNbMGiBql/kX4BgDoaZDcxnoLvN/GJKLEYECiqJRNm/oVLfVTwicbZPi04ziXDg0NGV39mjyGVUBU4EMOOeR3pJR/RESn1W/h2DHGBCKuB4DHx8fHr924cePbSS4HA5Kk2vbHUrcGTUxMXAIAJwHAAZMuMKnvGBUAGCwUCl9ft27dmzZHtw6ISq67u7tjxowZHy0WiwuklB9xHKcgpXy1o6PjzcHBwZdtTiBqLAYkqlLZtVN1NXv27AVENF/VFRFhsVh8c2Ji4u3x8fFXhoeHt9nOvimA2E7SRjwGxIaK7ReDAdFYc76KpSFWi5gyIBoLyYBoiNUipgyIxkIyIBpitYgpA6KxkAyIhlgtYsqAaCwkA6IhVouYMiAaC8mAaIjVIqYMiMZCtjognuctQsT967d1vEFEFSHEiIZELWfKgGgsaasC4nneBQCgnr2f6mZTtZfiPwRBMKAhVcuYMiAaS9lqgKi7ZYnoNkQ8LoIMfy2E+PMIdi1lwoBoLGcrAdLT07N7oVBQXUHnEYXvCCHU/VBtczAgGkvdSoB4nncHAHxeY/o7TM8RQqhdetviYEA0lrlVAPE872gAeExj6pNNNwkhGt12HjN09twYEI01aSFAbgWAL2lMfWfTTwshHjDwz40rA6KxVC0EyP8AQJfG1Hc2bZsv7AyIRpW0ECA1AChqTH1n0x8KIc4w8M+NKwOisVQtBIjRmz4AoF8IsUpDutyaMiAaS8eAvCcWA6JRN7kw5ScK318mz/O4g0SsWu4gEYVSZtxBuINolEu+TLmDcAeJU7HcQTRU4w7CHUSjXPJlyh2EO0iciuUOoqEadxDuIBrlki9T7iDcQeJULHcQDdW4g3AH0SiXfJlyB+EOEqdiuYNoqMYdhDuIRrnky5Q7CHeQOBXLHURDNe4g3EE0yiVfptxBuIPEqVjuIBqqcQfhDqJRLo1NlyxZMmtsbGwPx3GKHR0dv167dm21sVdzLLiDtE4HUXVVrVYXFAqFQrPrynoHKZVKHy0UChcS0an1N/R11rfiVbv/vAEADxaLxRsGBwc3NweFqaMyIPkGpK+vb0GtVrsYEVVdddSfiFT1q56OVLsk3yOlvLFSqbxus66sAtLb23uclPJKADhimiQlADyr3uQnhFC7lyZyMCD5BcT3/eOJSO2X3tegrl6RUp5dqVQetVVU1gCp70Sq3paxX8TkXpVSnlCpVH4Z0d7IjAHJJyCe5x2uHvEFgI9FLIB3pJRLbdWVFUB6e3s/JqW8q0HnmGp+G6rV6tLh4eEtEScf24wByR8g9dP1u4noSM2FX7dt27YTbeykbAUQ3/cvJaJva05CmUtEvCQIgpti+Gq5MCD5A8T3/S8RkTor0T6I6IthGP6LtuNODrYAWU1EJ8dMZkAIod7019SDAckXIEuXLp07Njb2fQBYGacwEPH+rVu3rjDdGtoYENd1fw8R/wMADoozEQB4rVqt7mM6kUZjMyD5AqSnp2e/QqEwCAB7NFrbXfz/ze3bt3etX7/+NzH933UzBsT3/VOI6D6DWOq3kT4hxEaTiTTyZUDyBYjv+0uJyOQqJxHRp8Iw/O9GtTHd/20Ach4R/ZNBEu8Q0efDMLzfIEZDVwYkd4B8pv7B23BtpzE4RgjxuEkAG4BcREQ3GiQxQkTnhmF4r0GMhq4MSL4A8TzvswBgVBOIuCwIgkcaFsc0BgyIhnp8L9Z7YjX9zYoMyPuFyR1EA1IAWCWEUD+cxT7y8GZFBiR5QNSVNpMXLkshRCF2VQKA7/vLiehukxiIuCIIgh+ZxPA8bzsAOAYx7hJCnGbg39CVAUkYEN/3ryeirzRcmV0b/EoIsa+BP/T29h4mpVxjEsNxnKVDQ0NPmsTwPO8lANgnboz6rreXxvWP4seAJAyI67qnIeKdURZnKhv1W08QBKfH9Vd+nufNAAB1W426GzXOoe6IniOEUHewxj5837+TiGJ3ACI6PQxD1ZGbdjAgCQNSLpeLo6OjrwLAb8VZVSJaHobhPXF8J/t4nqdufzgnZpx/FUKcG9P3PTfXdT+HiHFP096aO3fugoGBgQnTPKbzZ0ASBkQN57ruZYh4XYyFfUwIcWwMvw+5+L5/IBHF+lEUEQ8KguBpG3l4nqduCT9GNxYRfS0Mw7/V9dO1Z0BSAKR+mqO6gHroJurxlpTyiEqlMhzVoZGd7/vavx0h4sU2b+oslUrdjuM8odlR7xVCfK7R/Gz8nwFJCRA1rMY5+PNSyjMrlcpaG4s+OYbOHdCI+NUgCK63nUOpVFriOM4PAGBho9g2voM1GmOnU1H+obAuSCK/g+y8OK7rflkVHgDsP8XCjSPizZ2dnVeuWbNmVGdhdWzrV7X+AgBO2oXfA47jXGN61Wq6nNRds+Pj498iogsAQD0evfPxnHqUIQzDW3TmZmrLHSTFDjJ58dSnKCK6ADAfALaqx4HnzZv3yMDAwJjpIkf17+3tXUhE6ua8TygfRHwREdcMDQ09HzWGqV25XJ65efPm4wHggPq7BN4gorAZ3TNKrgxIRgCJslhsk7wCDAgDknzV5WhEBoQByVG5Jp8qA8KAJF91ORqRAWFAclSuyafKgDAgyVddjkZkQBiQHJVr8qkyIAxI8lWXoxEZEAYkR+WafKoMCAOSfNXlaEQGhAHJUbkmnyoDwoAkX3U5GpEBYUByVK7Jp8qAMCDJV12ORmRAGJAclWvyqTIgDEjyVZejERkQBiRH5Zp8qgwIA5J81eVoRAaEAclRuSafKgPCgCRfdTkakQFhQHJUrsmnyoAwIMlXXY5GZEAYkByVa/KpMiDvaz5KRGc3ewu25JeYRzRRwHXdkxFxtUmMTGzB5rruHyOiyVv3xtVb/Wxs+m4iJvtmSwHT7SrUbIjo2DAMHzOZmfEehRYmQgBwkxDiT0wmwr6tpYDrun+FiFeYzAoR+4IgWGcUw8RZ+fq+/wf17XrV5jBxj5cB4EAhxEjcAOzXOgr4vr8/Eal9GA8ymZXjOItMX99q3EHqG76rU6xuk8kQ0U87OztP23vvvUf6+/vVHnp8tKECrut2IeI1ALDScPoTUso9K5XK6yZxjAFZvHjxHsVi8ceI2GuSSN13kIhWO47zDBEZ52YhHw6RgAJEpD4Qd0NE9SH7abXXkYVhXysWi58cHBzcbBLLShF6nqfa4QqTRNiXFbCpABH9aGxs7Izh4WG1r2Pswwogrut+BRGtb/ASe1bs2PYKIOJZQRDcbiqEFUDqG8GoLrKnaULszwpYUGC7lHJRpVJ50TSWFUDqm6/cg4i/b5oQ+7MCFhR4YuHChUfbuNhjBRA1Ic/zzgaA7+1iGy8Lc+YQrEA0BYjolDAM749mPb2VNUC6u7s7Zs6ceR93ERvLwjEMFHhcCKG9vfWuxrMGiBrAdd0jEPE+za2FDbRgV1bggwoQkR+GobCli1VA1P6Tvu9fRUR/CQAFW0lyHFYgigKI+GdBEFwXxTaqjW1AoFwuF0dHR/8dAFZFTYLtWAELCtwhhPhDC3E+EMI6ICp6X1/fvFqtthoRj7KdMMdjBaZQ4KlqtXqU6Y+CUynbFEAmQfIQIh7GS8oKNFGBgWKxePrg4OCrzRijaYCoZLu6ujp33333W4nozGYkzzHbXoE7qtXquc3oHDuUbSogapCVK1cWXnjhhcuJ6EoA6Gj7JWUBbClwuRDib2wF21WcpgOyY+BSqbSkUChcrZ7yavakOH5LK/A4EV1m81LudGolBsiObvL888+fSERXIaIHAE5LLyVPzpYCREQ/B4BrwzD8ia2gUeIkCsiOhNRp17PPPjuvs7PzBCI6DgCWAcB8AJgVJWm2aXkFqgDwEiI+KaV8mIgGu7q6XrZxb5WucqkAMjlJz/PUo7q7SSk7HMcpAsC+iLi3+tFRdzJsn18FpJTjiPialPL/HMd5p1qtjs2aNWtcCFFLc1ZchGmqz2NnXgEGJPNLxAmmqQADkqb6PHbmFWBAMr9EnGCaCjAgaarPY2deAQYk80vECaapAAOSpvo8duYVYEAyv0ScYJoKMCBpqs9jZ14BBiTzS8QJpqkAA5Km+jx25hVgQDK/RJxgmgowIGmqz2NnXgEGJPNLxAmmqQADkqb6PHbmFWBAMr9EnGCaCvw/RjCpfZBsujgAAAAASUVORK5CYII=";

        /// <summary>
        /// 获取发行日期
        /// </summary>
        /// <returns>发行日期</returns>
        public DateTime GetPublishDate() => DateTime.Now;

        /// <summary>
        /// 获取最近更新日期
        /// </summary>
        /// <returns>最近更新日期</returns>
        public DateTime GetLastUpdateDate() => DateTime.Now;

        /// <summary>
        /// 获取控制器
        /// </summary>
        /// <returns>控制器</returns>
        public IController GetController() => controller;

        /// <summary>
        /// 指示是否是市场版本
        /// </summary>
        /// <returns>是否是市场版本</returns>
        public bool IsMarketVersion() => false;

        /// <summary>
        /// 获取市场版本插件协议
        /// </summary>
        /// <returns>市场版本插件协议</returns>
        public IMarketPluginContract GetMarketPluginContract() => null;

        /// <summary>
        /// 获取根启动文件名称
        /// </summary>
        /// <returns>根启动文件名称</returns>
        public string GetRootStartupFileName() => "ResourseMonitor.dll";
        #endregion

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Window_Closed(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }
    }
}
