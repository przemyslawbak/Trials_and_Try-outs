using System.Threading;
using System.Threading.Tasks;

namespace BasicConfig.Infrastructure
{
    //crone expressions: https://crontab.guru/#*_*/6_*_*_*
    public class DisplaySomeCrapTask : IScheduledTask
    {
        public string Schedule => "*/1 * * * *";

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Number.Current.Integer = Number.Current.Integer + 1;
            await Task.Delay(100);
        }
    }

    public class Number
    {
        public static Number Current { get; set; }

        static Number()
        {
            Current = new Number { Integer = 0 };
        }

        public int Integer { get; set; }
    }
}
