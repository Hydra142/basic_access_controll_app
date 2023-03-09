using SafeMessenge.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Services;

public class ActivationService : IActivationService
{
    private AppDataService _appDataService { get; set; }
    public ActivationService(AppDataService appDataService)
    {
        _appDataService = appDataService;
    }
    public async Task ActivateAsync()
    {
        var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "TBD/Data");
        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }
        await _appDataService.InitAsync();
    }
}

