namespace chabcav.application.Queries.Dictionary
{
    public class DictionarySearchResultDto
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public DateTime UploadedAt { get; set; }
    public string MatchSnippet { get; set; } // <--- this is important
}

}
