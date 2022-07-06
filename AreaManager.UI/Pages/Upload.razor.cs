using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AreaManager.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ExcelDataReader;
using System.Data;
using System.IO;
using AreaManager.Data.Entities;

namespace AreaManager.UI.Pages;

public partial class Upload
{
    [Inject]
    protected AreaManagerContext LicenseDbContext { get; set; }

    public async Task CopyStream(Stream stream, string destPath)
    {
        using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
        {
            await stream.CopyToAsync(fileStream);
        }
    }

    private async Task UploadFiles()
    {
        try
        {
            progress = true;
            uploadErrors.Clear();
            uploadedSuccessfully.Clear();

            foreach (var browserFile in files)
            {
                try
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


                    var file = Path.GetTempFileName();
                    await CopyStream(browserFile.OpenReadStream(), file);

                    var stream = File.Open(file, FileMode.Open, FileAccess.Read);

                    IExcelDataReader reader;

                    if (browserFile.Name.EndsWith(".xls"))
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else if (browserFile.Name.EndsWith(".xlsx"))
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    else
                        throw new Exception("The file to be processed is not an Excel file");
                    
                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };
                    var dataSet = reader.AsDataSet(conf);

                    // Now you can get data from each sheet by its index or its "name"
                    var dataTable = dataSet.Tables[0];
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {

                        var plotno = (string) dataTable.Rows[i][0];
                        var longitude = (string) dataTable.Rows[i][1];
                        var latitude = (string) dataTable.Rows[i][2];
                        var publicplace = (string) dataTable.Rows[i][3];
                        var postcode = ((double) dataTable.Rows[i][4]).ToString();
                        var village = (string) dataTable.Rows[i][5];
                        var street = (string) dataTable.Rows[i][6];
                        var town = (string) dataTable.Rows[i][7];
                        var district = (string) dataTable.Rows[i][8];
                        var state = (string) dataTable.Rows[i][9];
                        var country = (string) dataTable.Rows[i][10];
                        var updated_by = (string) dataTable.Rows[i][11];

                        var areadata = new AreaData(
                            plotno,
                            longitude,
                            latitude,
                            publicplace,
                            postcode,
                            village,
                            street,
                            town,
                            district,
                            state,
                            country,
                            updated_by
                        );
                        
                        await LicenseDbContext.AreaDatas.AddAsync(areadata);
                        await LicenseDbContext.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    uploadErrors.Add($"{browserFile.Name}: {e.Message}");
                }
            }

            files.Clear();
        }
        catch (Exception e)
        {
            uploadErrors.Add(e.Message);
        }
        finally
        {
            progress = false;
        }
    }

    private bool clearing;
    private bool progress;
    private static string defaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
    private string dragClass = defaultDragClass;
    private List<IBrowserFile> files = new();
    private readonly List<string> uploadErrors = new();
    private readonly List<string> uploadedSuccessfully = new();

    private void OnInputFileChanged(InputFileChangeEventArgs e)
    {
        try
        {
            ClearDragClass();
            ClearMessages();
            files = e.GetMultipleFiles(maximumFileCount: 500).ToList();
        }
        catch (Exception exception)
        {
            uploadErrors.Add(exception.Message);
        }
    }

    private async Task Clear()
    {
        clearing = true;
        ClearMessages();
        files.Clear();
        ClearDragClass();
        await Task.Delay(100);
        clearing = false;
    }

    private void ClearMessages()
    {
        uploadedSuccessfully.Clear();
        uploadErrors.Clear();
    }

    private void SetDragClass() => dragClass = $"{defaultDragClass} mud-border-primary";

    private void ClearDragClass() => dragClass = defaultDragClass;
}