using System;
using System.Collections.Generic;
using System.IO;
using AreaManager.Data.Enums;

namespace AreaManager.Data.Entities;

public class AreaData
{
    public Guid Id { get; private set; }
    public string PlotNo { get; private set; }
    public string Longitude { get; private set; }
    public string Latitude { get; private set; }
    public string PostCode { get; private set; }
    public string Village { get; private set; }
    public string Street { get; private set; }
    public string Town { get; private set; }
    public string District { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    public string Updated_by { get; private set; }
    public string PublicPlace { get; private set; }

    public AreaData()
    {

    }
    public AreaData(string plotno, string latitude, string longitude, string publicplace, string postcode, string village, string street, string town, string district, string state, string country, string updated_by)
    {
        Id = Guid.NewGuid();
        PlotNo = plotno;
        Longitude = longitude;
        Latitude = latitude;
        PublicPlace = publicplace;
        PostCode = postcode;
        Village = village;
        Street = street;
        Town = town;
        District = district;
        State = state;
        Country = country;
        Updated_by = updated_by;
    }

}