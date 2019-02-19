using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQtoCSV;

namespace Launcher.Model
{
    class Leader
    {
        [CsvColumn(FieldIndex = 0, CanBeNull = false)]
        public int LeaderID { get; set; }
        [CsvColumn(FieldIndex = 1, CanBeNull = false)]
        public string Name { get; set; }
        [CsvColumn(FieldIndex = 2)]
        public int Female { get; set; }
        [CsvColumn(FieldIndex = 3)]
        public string Alias { get; set; }
        [CsvColumn(FieldIndex = 4)]
        public string Corps { get; set; }
        [CsvColumn(FieldIndex = 5)]
        public string Graph { get; set; }
        [CsvColumn(FieldIndex = 6)]
        public int Status { get; set; }
        [CsvColumn(FieldIndex = 7)]
        public int Power { get; set; }
        [CsvColumn(FieldIndex = 8)]
        public int History { get; set; }
        [CsvColumn(FieldIndex = 9)]
        public string Faction { get; set; }
        [CsvColumn(FieldIndex = 10)]
        public int Will { get; set; }
        [CsvColumn(FieldIndex = 11)]
        public int Loyality { get; set; }
        [CsvColumn(FieldIndex = 12)]
        public int UnitIndex { get; set; }
        [CsvColumn(FieldIndex = 13)]
        public int Icon { get; set; }
        [CsvColumn(FieldIndex = 14)]
        public int RecruitType { get; set; }
        [CsvColumn(FieldIndex = 15)]
        public int RecruitValue { get; set; }
        [CsvColumn(FieldIndex = 16)]
        public string LeaderSkill0 { get; set; }
        [CsvColumn(FieldIndex = 17)]
        public string LeaderSkill1 { get; set; }
        [CsvColumn(FieldIndex = 18)]
        public string LeaderSkill2 { get; set; }
        [CsvColumn(FieldIndex = 19)]
        public string LeaderSkill3 { get; set; }
        [CsvColumn(FieldIndex = 20)]
        public string LeaderSkill4 { get; set; }
        [CsvColumn(FieldIndex = 21)]
        public string LeaderSkill5 { get; set; }
        [CsvColumn(FieldIndex = 22)]
        public string Description0 { get; set; }
        [CsvColumn(FieldIndex = 23)]
        public string Description1 { get; set; }
        [CsvColumn(FieldIndex = 24)]
        public string Description2 { get; set; }
        [CsvColumn(FieldIndex = 25)]
        public string Description3 { get; set; }
        [CsvColumn(FieldIndex = 26)]
        public int Cabinet { get; set; }
        [CsvColumn(FieldIndex = 27)]
        public string CDes { get; set; }
        [CsvColumn(FieldIndex = 28)]
        public string CDes2 { get; set; }
        [CsvColumn(FieldIndex = 29)]
        public int Cond1 { get; set; }
        [CsvColumn(FieldIndex = 30)]
        public string Cond2 { get; set; }
        [CsvColumn(FieldIndex = 31)]
        public string Cond3 { get; set; }
        [CsvColumn(FieldIndex = 32)]
        public string Feat1 { get; set; }
        [CsvColumn(FieldIndex = 33)]
        public string Feat2 { get; set; }
        [CsvColumn(FieldIndex = 34)]
        public string Feat3 { get; set; }
        public Leader()
        {
            Female = 1;
            Status = -1;
            Power = 10;
            History = 0;
            Faction = "無";
            Will = 1;
            Loyality = 10;
            UnitIndex = -10;
            Icon = 0;
            RecruitType = 0;
            RecruitValue = 0;
            Cabinet = 0;
            Cond1 = 100;
        }
    }
}
