using MagazineCMS.DataAccess.Data;
using MagazineCMS.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace MagazineCMS.Areas.Manager.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DownloadController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[HttpGet]
        //public IActionResult DownloadMagazineContributions(int magazineId)
        //{
        //    // Get list Contribution have same MagazineId
        //    var contributions = _context.Contributions.Where(c => c.MagazineId == magazineId).ToList();

        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        //        {
        //            foreach (var contribution in contributions)
        //            {
        //                foreach (var document in contribution.Documents)
        //                {
        //                    // Get url
        //                    var filePath = document.DocumentUrl;

        //                    // Check url and then download
        //                    if (System.IO.File.Exists(filePath))
        //                    {
        //                        // Add file to zip
        //                        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        //                        var entry = archive.CreateEntry(document.DocumentUrl);
        //                        using (var entryStream = entry.Open())
        //                        using (var fileStream = new MemoryStream(fileBytes))
        //                        {
        //                            fileStream.CopyTo(entryStream);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return File(memoryStream.ToArray(), "application/zip", $"Magazine_{magazineId}_Contributions.zip");
        //    }
        //}

        [HttpGet("download/magazine/{magazineId}")]
        public IActionResult DownloadAllDocumentsForMagazine(int magazineId)
        {
            // Lấy tất cả các đóng góp cho tạp chí cụ thể
            var contributions = _unitOfWork.Contribution.GetAll()
                .Where(c => c.MagazineId == magazineId)
                .ToList();

            if (contributions.Any())
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var contribution in contributions)
                        {
                            // Lấy tất cả các tài liệu cho mỗi đóng góp
                            var documents = _unitOfWork.Document.GetAll()
                                .Where(d => d.ContributionId == contribution.Id)
                                .ToList();

                            foreach (var document in documents)
                            {
                                var filePath = document.DocumentUrl;
                                if (System.IO.File.Exists(filePath))
                                {
                                    var documentBytes = System.IO.File.ReadAllBytes(filePath);
                                    var fileName = Path.GetFileName(filePath);
                                    var entry = archive.CreateEntry(fileName);
                                    using (var entryStream = entry.Open())
                                    {
                                        entryStream.Write(documentBytes, 0, documentBytes.Length);
                                    }
                                }
                            }
                        }
                    }

                    return File(memoryStream.ToArray(), "application/zip", "Documents.zip");
                }
            }

            // Nếu không tìm thấy tài liệu, trả về NotFound
            return NotFound();
        }




    }
}
