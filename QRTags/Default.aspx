<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QRTags._Default" %>

<!DOCTYPE html>

<html lang="<asp:Literal runat="server" Text="<%$ Resources:LocalizedText, lang %>" />">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="description" content="Generate QR codes for use with Epi Info.">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0">

    <!-- Add to homescreen for Chrome on Android -->
    <meta name="mobile-web-app-capable" content="yes">

    <!-- Add to homescreen for Safari on iOS -->
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="apple-mobile-web-app-title" content="Print QR Codes">

    <!-- Tile icon for Win8 (144x144 + tile color) -->
    <meta name="msapplication-TileColor" content="#3372DF">
    
    <link rel="stylesheet" href="https://unpkg.com/material-components-web@latest/dist/material-components-web.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons" />
    <link rel="stylesheet" type="text/css" href="~/Content/StyleSheet.css?v=57" />
    <title><asp:Literal runat="server" Text="<%$ Resources:LocalizedText, title %>" /></title>

    <script src="https://unpkg.com/material-components-web@latest/dist/material-components-web.js"></script>
    <script type="text/javascript">
        window.mdc.autoInit();
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="no-print">
            <header class="mdc-toolbar mdc-toolbar--fixed">
                <div class="mdc-toolbar__row">
                    <section class="mdc-toolbar__section mdc-toolbar__section--align-start">
                        <span class="mdc-toolbar__title"><asp:Literal runat="server" Text="<%$ Resources:LocalizedText, title %>" /></span>
                    </section>
                    <section class="mdc-toolbar__section mdc-toolbar__section--align-end" role="toolbar">
                        <asp:LinkButton CssClass="material-icons mdc-toolbar__icon" aria-label="Refresh" ID="refresh" runat="server" Text="autorenew" />
                        <a href="#" class="material-icons mdc-toolbar__icon" aria-label="Print" alt="Print" onclick="window.print();return false">print</a>
                    </section>
                </div>
            </header>
            <div class="mdc-toolbar-fixed-adjust">
                <div class="spacer"></div>
                <div class="mdc-card my-card-container">
                    <div class="mdc-card__primary-action">
                            <h3 class="mdc-typography--title"><asp:Literal runat="server" Text="<%$ Resources:LocalizedText, settings %>" /></h3>
                            <h3 class="mdc-typography--subheading1"><asp:Literal runat="server" Text="<%$ Resources:LocalizedText, pages %>" /></h3>
                            <asp:DropDownList ID="cbxPages" runat="server" AutoPostBack="true" />
                            <h3 class="mdc-typography--subheading1"><asp:Literal runat="server" Text="<%$ Resources:LocalizedText, logo %>" /></h3>
                            <div class="mdc-form-field mdc-form-field--align-end">
                                <asp:FileUpload ID="uploader" runat="server" onchange="this.form.submit();" />
                            </div>
                    </div>
                    <div class="mdc-card__actions">
                        <div class="mdc-card__action-buttons">
                            <asp:Button runat="server" CssClass="mdc-button mdc-card__action mdc-card__action--button" Text="<%$ Resources:LocalizedText, refresh %>"/>
                            <button class="mdc-button mdc-card__action mdc-card__action--button" onclick="window.print();return false"><asp:Literal runat="server" Text="<%$ Resources:LocalizedText, print %>" /></button>
                        </div>
                    </div>
                </div>
                <div class="spacer"></div>
            </div>
        </div>
        <div id="output" runat="server"></div>
    </form>
</body>
</html>
