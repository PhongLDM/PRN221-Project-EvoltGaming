using ClosedXML.Excel;
using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvoltingStore.Pages.Management
{
    public class GamesModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        public IActionResult OnGet()
        {
            String userJSON = (String)HttpContext.Session.GetString("user");
            User user = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            if (user.RoleId == 1)
            {
                List<Game> games = context.Games.ToList();

                ViewData["games"] = games;

                return Page();
            }

            return Redirect("/Error");
        }

        public IActionResult OnPostRemove(int gameId)
        {
            String userJSON = (String)HttpContext.Session.GetString("user");
            User user = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            if (user.RoleId == 1)
            {
                Game g = context.Games.FirstOrDefault(g => g.GameId == gameId);

                context.Remove(g);
                context.SaveChanges();

                context = new EvoltingStoreContext();

                List<Game> games = context.Games.ToList();

                ViewData["games"] = games;

                return Page();
            }

            return Redirect("/Error");
        }

        public IActionResult OnPostExportToExcel()
        {
            String userJSON = (String)HttpContext.Session.GetString("user");
            User user = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            List<Game> games = context.Games.ToList();
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Games");
                    var currentRow = 1;

                    // Header
                    worksheet.Cell(currentRow, 1).Value = "Name";
                    worksheet.Cell(currentRow, 2).Value = "Description";
                    worksheet.Cell(currentRow, 3).Value = "GameSource";
                    worksheet.Cell(currentRow, 4).Value = "ReleaseDate";
                    worksheet.Cell(currentRow, 5).Value = "UpdateDate";
                    worksheet.Cell(currentRow, 6).Value = "Platform";
                    worksheet.Cell(currentRow, 7).Value = "Image";
                    worksheet.Cell(currentRow, 8).Value = "Price";

                    // Data
                    foreach (var game in games)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = game.Name;
                        worksheet.Cell(currentRow, 2).Value = game.Description;
                        worksheet.Cell(currentRow, 3).Value = game.GameSource;
                        worksheet.Cell(currentRow, 4).Value = game.ReleaseDate;
                        worksheet.Cell(currentRow, 5).Value = game.UpdateDate;
                        worksheet.Cell(currentRow, 6).Value = game.Platform;
                        worksheet.Cell(currentRow, 7).Value = game.Image;
                        worksheet.Cell(currentRow, 8).Value = game.Price;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GamesList.xlsx");
                    }
                }
            }
        }
    }

