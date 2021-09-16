namespace IMP.Application.Models.File
{
    public class UploadFileResponse
    {
        public UploadFileResponse(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
    }
}