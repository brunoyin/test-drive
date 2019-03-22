
#l query2csv.cake

using System;
using System.Collections.Generic;

using System.Linq;  
using System.Xml;  
using System.Xml.Linq;  
using System.IO;  

var target = Argument("target", "Default");

string nullable2str(decimal? v){
    var ret = "";
    if (v != null){ ret = $"{v:#,###.#0}"; }
    return ret;
}

Task("Default")
    .Does(async () => {
        Information("Running Postgres query");
        var connectionString = string.Format("Host={0};Database={1};Username={2};Password={3}",
            Environment.GetEnvironmentVariable("PGHOST") ?? "192.168.0.48",
            Environment.GetEnvironmentVariable("PGDATABASE") ?? "bruno",
            Environment.GetEnvironmentVariable("PGUSER") ?? "bruno",
            Environment.GetEnvironmentVariable("PGPASSWORD") ?? "bruno"
        );
        var colleges = await GetColleges(1, connectionString);
        Information($"Total found: {colleges.Count} collges");

        var xdoc = new XElement("region", new XAttribute("id", "1"),
            from data in colleges
            group data by ((College)data).State into regionData
            select new XElement("state", new XAttribute("id", regionData.Key),
                from item in regionData
                select new XElement("college", 
                    new XAttribute("name", item.Name),
                    new XAttribute("admrate", nullable2str(item.AdmRate)),
                    new XAttribute("sat", nullable2str(item.SatAvg)),
                    new XAttribute("act", nullable2str(item.ActAvg)),
                    new XAttribute("cost", nullable2str(item.Cost)),
                    new XAttribute("students", nullable2str(item.Enrollments))
                )
            )
        );
        var writer = XmlWriter.Create("college-region-state.xml");
        // Console.WriteLine(xdoc);
        xdoc.WriteTo(writer);
        writer.Close();
    }
);

RunTarget(target);
