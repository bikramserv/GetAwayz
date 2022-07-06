using System.Collections.Generic;
using System.Threading.Tasks;
using AreaManager.Data;
using AreaManager.Data.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AreaManager.UI.Pages;

public partial class Index
{
    [Inject]
    private AreaManagerContext Context { get; set; }
    private List<AreaData> Areas = new List<AreaData>();

    protected override async Task OnInitializedAsync()
    {
      Areas = await Context.AreaDatas.ToListAsync();
    }
}