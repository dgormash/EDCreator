namespace FDCreator.Misc
{
    public class StabilizerParsedData:ParsedData
    {
        //Ещё раз воспользовался принципами ООП. Это класс, производный от ParsedData, он содержи те же поля,
        //что и ParsedData и плюс эти. Но чтобы получить к ним доступ надо... (см. файл StabilizerExcelProcessor)
        public string StabilizerOd { get; set; }
        public string LobeLength { get; set; }
        public string LobeWidth { get; set; }
        public string FishingNeckTongSpace { get; set; }
    }
}