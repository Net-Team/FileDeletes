using Topshelf;

namespace FileDeletes
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<FileDeletesControl>();
                x.RunAsLocalSystem();
                x.SetServiceName(nameof(FileDeletes));
                x.SetDisplayName(nameof(FileDeletes));
                x.SetDescription(nameof(FileDeletes));
            });
        }
    }
}
