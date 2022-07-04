using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Download;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Profile;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peernet.Browser.Application.Extensions;
using System;
using System.Collections.ObjectModel;

namespace Peernet.Browser.Application.ViewModels
{
    public class SharedDataViewModel : ViewModelBase
    {
        private readonly IDownloadManager downloadManager;
        private readonly IEnumerable<IPlayButtonPlug> playButtonPlugs;
        private ObservableCollection<Comment> comments;

        public ObservableCollection<Comment> Comments
        {
            get => comments;
            set
            {
                comments = value;
                OnPropertyChanged(nameof(Comments));
            }
        }

        public SharedDataViewModel(User user, IDownloadManager downloadManager, IEnumerable<IPlayButtonPlug> playButtonPlugs)
        {
            Files = user.Files.Select(f => new DownloadModel(f)).ToList();
            this.downloadManager = downloadManager;
            this.playButtonPlugs = playButtonPlugs;
            Files.SetPlayerState(playButtonPlugs);
        }

        public IAsyncCommand<DownloadModel> DownloadCommand =>
            new AsyncCommand<DownloadModel>(
                async downloadModel =>
                {
                    await downloadManager.QueueUpDownload(downloadModel);
                });

        public List<DownloadModel> Files { get; set; }

        public IAsyncCommand<DownloadModel> StreamFileCommand =>
            new AsyncCommand<DownloadModel>(
                model =>
                {
                    playButtonPlugs.Foreach(async plug =>
                    {
                        if (plug?.IsSupported(model.File) == true)
                        {
                            await plug?.Execute(model.File);
                        }
                    });

                    return Task.CompletedTask;
                });

        public IAsyncCommand<DownloadModel> GetCommentsCommand =>
            new AsyncCommand<DownloadModel>(
                model =>
                {
                    Comments = new()
                    {
                        new Comment
                        {
                            Id = Guid.NewGuid().ToString(),
                            Username = "Lucy",
                            Content = "This is amazing!"
                        },
                        new Comment
                        {
                            Id = Guid.NewGuid().ToString(),
                            Username = "Jake",
                            Content = "There should be more people like this!"
                        }
                    };

                    return Task.CompletedTask;
                });
    }

    public class Comment
    {
        public string Id { get; set; }

        public string Username { get; set; }
        
        public string Content { get; set; }

        public DateTime Date { get; set; } = DateTime.Now; 
    }
}