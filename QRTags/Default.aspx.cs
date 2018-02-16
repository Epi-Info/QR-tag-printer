using System;
using System.Drawing;
using QRCoder;
using System.Text;
using System.IO;
using Resources;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace QRTags
{
    public partial class _Default : System.Web.UI.Page
    {

        string logoBase64;
        string logoExt;
        Bitmap mainLogo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mainLogo = (Bitmap)Bitmap.FromFile(Path.Combine(Server.MapPath("~/App_Data"), "vector2.png"));
                Session["mainLogo"] = mainLogo;
                cbxPages.Items.Add(LocalizedText.one_page);
                cbxPages.Items.Add(LocalizedText.two_pages);
                cbxPages.Items.Add(LocalizedText.three_pages);
            }
            else
            {
                LoadUserLogo();
            }

            GenerateQRCodes();

        }

        private void GenerateQRCodes()
        {
            long time = (long)Math.Round(DateTime.Now.Subtract(DateTime.MinValue.AddYears(2016).AddMonths(11).AddDays(6)).TotalMilliseconds);

            QRCodeGenerator gen = new QRCodeGenerator();

            int pageCount = cbxPages.SelectedIndex + 1;

            StringBuilder sb = new StringBuilder();
            for (int z = 0; z < pageCount; z++)
            {
                sb.Append("<table class=\"expand\">");
                for (int x = 0; x < 9; x++)
                {
                    sb.Append("<tr>");
                    for (int y = 0; y < 4; y++)
                    {
                        sb.Append("<td>");
                        String code = ToBase36(time++);
                        QRCodeData data = gen.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);

                        var imgType = Base64QRCode.ImageType.Png;
                        Base64QRCode qrCode = new Base64QRCode(data);
                        string qrCodeImageAsBase64 = qrCode.GetGraphic(3, Color.Black, Color.White, mainLogo, 15, 1, true, imgType);
                        var htmlPictureTag = $"<img alt=\"QR Code representing {data}\" src=\"data:image/{imgType.ToString().ToLower()};base64,{qrCodeImageAsBase64}\" />";

                        sb.Append(htmlPictureTag);
                        sb.Append("</td><td class=\"cell\">");
                        if (!string.IsNullOrEmpty(logoBase64))
                        {
                            var logoTag = $"<img alt=\"Organization logo\" src=\"data:image/{logoExt.ToLower()};base64,{logoBase64}\" />";
                            sb.Append(logoTag + "<br/>");
                        }
                        sb.Append(code + "     ");
                        sb.Append("</td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                if (z != pageCount - 1)
                {
                    sb.Append("<div class=\"page-break\"/>");
                }
            }
            this.output.InnerHtml = sb.ToString();
        }

        private void LoadUserLogo()
        {
            mainLogo = (Bitmap)Session["mainLogo"];
            if (Session["logoBase64"] != null)
            {
                logoBase64 = Session["logoBase64"].ToString();
                logoExt = Session["logoExt"].ToString();
            }

            if (uploader.HasFile)
            {
                bool fileOK = false;
                String fileExtension = Path.GetExtension(uploader.FileName).ToLower();
                String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                        logoExt = fileExtension.Trim('.');
                        Session["logoExt"] = logoExt;
                    }
                }
                if (fileOK)
                {
                    Bitmap logo = (Bitmap)(Resize(Bitmap.FromStream(uploader.PostedFile.InputStream), 30, 30));
                    if (logo != null)
                    {
                        MemoryStream ms = new MemoryStream();
                        logo.Save(ms, GetImageFormat(logoExt));
                        byte[] byteImage = ms.ToArray();
                        logoBase64 = Convert.ToBase64String(byteImage);
                        Session["logoBase64"] = logoBase64;
                    }
                }
            }
        }

        private static string ToBase36(long value)
        {
            const string base36 = "0123456789abcdefghijklmnopqrstuvwxyz";
            var sb = new StringBuilder(13);
            do
            {
                sb.Insert(0, base36[(byte)(value % 36)]);
                value /= 36;
            } while (value != 0);
            return sb.ToString();
        }

        private ImageFormat GetImageFormat(string extension)
        {
            if (extension.ToLower().Equals("png"))
                return ImageFormat.Png;
            else if (extension.ToLower().Equals("gif"))
                return ImageFormat.Gif;
            else
                return ImageFormat.Jpeg;
        }

        private static System.Drawing.Image Resize(System.Drawing.Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Transparent);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
    }
}