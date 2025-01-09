using AdaKurumsal.DataLayer;
using AdaKurumsal.Models.FileManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace AdaKurumsal.Pages.Admin.FileManagement
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly EFContext _context;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty(SupportsGet = true)]
        public string FolderId { get; set; }

        public Folder CurrentFolder { get; set; }
        public List<Folder> Folders { get; set; }
        public List<Image> Images { get; set; }
        public List<Folder> BreadcrumbFolders { get; set; }

        public IndexModel(IWebHostEnvironment environment, EFContext context, ILogger<IndexModel> logger)
        {
            _environment = environment;
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            await LoadFolderData();
            return Page();
        }
        private async Task LoadFolderData()
        {
            Guid? currentFolderId = string.IsNullOrEmpty(FolderId) ? null : Guid.Parse(FolderId);

            // Get current folder if ID is provided
            CurrentFolder = currentFolderId.HasValue
                ? await _context.Folders.FindAsync(currentFolderId)
                : null;

            // Get breadcrumb
            BreadcrumbFolders = new List<Folder>();
            if (CurrentFolder != null)
            {
                var folder = CurrentFolder;
                while (folder != null)
                {
                    BreadcrumbFolders.Insert(0, folder);
                    folder = await _context.Folders
                        .FirstOrDefaultAsync(f => f.Id == folder.ParentFolderId);
                }
            }

            // Get folders in current directory
            Folders = await _context.Folders
                .Where(f => f.ParentFolderId == currentFolderId && f.IsActive)
                .OrderBy(f => f.Name)
                .ToListAsync();

            // Get images in current directory
            Images = await _context.Images
                .Where(i => i.FolderId == currentFolderId && i.IsActive)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }
        public async Task<IActionResult> OnPostCreateFolderAsync([FromForm] string folderName, [FromForm] string parentFolderId)
        {
            if (string.IsNullOrWhiteSpace(folderName))
                return BadRequest("Folder name is required.");

            Guid? parentId = !string.IsNullOrEmpty(parentFolderId) ? Guid.Parse(parentFolderId) : null;

            var folder = new Folder
            {
                Name = folderName,
                ParentFolderId = parentId,
                Path = await GenerateFolderPath(parentId, folderName),
                CreatedDate = DateTime.UtcNow,
                CreatedBy = User.Identity.Name,

                IsActive = true
            };

            _context.Folders.Add(folder);
            await _context.SaveChangesAsync();

            // Create physical folder
            var physicalPath = Path.Combine(_environment.WebRootPath, "uploads", folder.Path);
            Directory.CreateDirectory(physicalPath);

            return RedirectToPage(new { folderId = parentFolderId });
        }
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            if (files == null || !files.Any())
                return BadRequest("No files were uploaded.");

            Guid? currentFolderId = !string.IsNullOrEmpty(FolderId) ? Guid.Parse(FolderId) : null;
            var currentFolder = currentFolderId.HasValue ? await _context.Folders.FindAsync(currentFolderId) : null;
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", currentFolder?.Path ?? "");

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // Generate unique filename
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(uploadPath, uniqueFileName);

                    // Create directory if it doesn't exist
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    // Save file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Save to database
                    var image = new Image
                    {
                        Name = file.FileName,
                        Path = $"/uploads/{(currentFolder?.Path ?? "")}/{uniqueFileName}".Replace("//", "/"),
                        FolderId = currentFolderId,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true
                    };

                    _context.Images.Add(image);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage(new { folderId = FolderId });
        }
        public async Task<IActionResult> OnPostRenameFolderAsync([FromBody] RenameFolderModel model)
        {
            var folder = await _context.Folders.FindAsync(Guid.Parse(model.FolderId));
            if (folder == null)
                return NotFound();

            folder.Name = model.NewName;
            folder.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
        public async Task<IActionResult> OnPostRenameImageAsync([FromBody] RenameImageModel model)
        {
            var image = await _context.Images.FindAsync(Guid.Parse(model.ImageId));
            if (image == null)
                return NotFound();

            image.Name = model.NewName;
            image.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
        public async Task<IActionResult> OnPostDeleteFolderAsync([FromBody] DeleteFolderModel model)
        {
            var folder = await _context.Folders
                .Include(f => f.SubFolders)
                .Include(f => f.Images)
                .FirstOrDefaultAsync(f => f.Id == Guid.Parse(model.FolderId));

            if (folder == null)
                return NotFound();

            // Soft delete folder and all contents
            await SoftDeleteFolderRecursive(folder);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
        private async Task SoftDeleteFolderRecursive(Folder folder)
        {
            // Mark folder as inactive
            folder.IsActive = false;
            folder.UpdatedDate = DateTime.UtcNow;

            // Mark all images in folder as inactive
            foreach (var image in folder.Images)
            {
                image.IsActive = false;
                image.UpdatedDate = DateTime.UtcNow;
            }

            // Recursively mark all subfolders and their contents as inactive
            foreach (var subFolder in folder.SubFolders)
            {
                await SoftDeleteFolderRecursive(subFolder);
            }
        }
        public async Task<IActionResult> OnPostDeleteImageAsync([FromBody] DeleteImageModel model)
        {
            var image = await _context.Images.FindAsync(Guid.Parse(model.ImageId));
            if (image == null)
                return NotFound();

            image.IsActive = false;
            image.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
        private async Task<string> GenerateFolderPath(Guid? parentId, string folderName)
        {
            if (!parentId.HasValue)
                return folderName;

            var parent = await _context.Folders.FindAsync(parentId);
            return Path.Combine(parent?.Path ?? "", folderName);
        }
    }
    public class RenameFolderModel
    {
        [Required]
        public string FolderId { get; set; }
        [Required]
        public string NewName { get; set; }
    }

    public class RenameImageModel
    {
        [Required]
        public string ImageId { get; set; }
        [Required]
        public string NewName { get; set; }
    }

    public class DeleteFolderModel
    {
        [Required]
        public string FolderId { get; set; }
    }

    public class DeleteImageModel
    {
        [Required]
        public string ImageId { get; set; }
    }
}
