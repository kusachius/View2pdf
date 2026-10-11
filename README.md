# Kusachius.View2Pdf

`Kusachius.View2Pdf` provides services for rendering Razor views to strings and
converting HTML to PDF through a configured conversion service.

## Installation

```shell
dotnet add package Kusachius.View2Pdf
```

## Usage

Register `ViewRenderService` with the ASP.NET Core services it requires, then
use `IViewRenderService.RenderToStringAsync` to render a Razor view. Use
`IHtml2PdfService.Html2PdfAsync` or `HtmlFile2PdfStreamAsync` to send HTML to
the conversion service configured by the `PDF_CONVERT_URL` environment variable.
If that variable is not set, the default endpoint is `http://127.0.0.1:3333`.
Docker image openlabs/docker-wkhtmltopdf-aas can be used as this service.

## Publishing

The GitHub Actions workflow in [`.github/workflows/publish.yml`](.github/workflows/publish.yml)
builds and publishes the package to NuGet.org.

Before publishing, configure a NuGet.org trusted publisher for the GitHub
repository hosting this solution and the `publish.yml` workflow file. Add the
NuGet.org package-owner username as a GitHub Actions secret named `NUGET_USER`.
Then push a version tag, for example `v1.4.0`.

The workflow uses GitHub OIDC and `NuGet/login@v1`; it does not require a
long-lived NuGet API key.
