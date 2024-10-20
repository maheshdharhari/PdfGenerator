# PDF Generator with PuppeteerSharp

This project provides a simple C# console application that generates PDF files from web pages using the PuppeteerSharp library. The application navigates to a specified URL and saves the rendered page as a PDF file.

## Features

- Generates PDFs from web pages.
- Configurable retry mechanism for handling transient errors.
- Customizable PDF options, including margins, scale, and background printing.

## Prerequisites

- .NET 5.0 or later
- Chrome or Chromium installed on your machine
- PuppeteerSharp library

## Installation

1. **Clone the repository** or download the source code.
   ```bash
   git clone https://github.com/yourusername/pdf-generator.git
   cd pdf-generator
   ```

2. **Install the PuppeteerSharp NuGet package**:
   ```bash
   dotnet add package PuppeteerSharp
   ```

3. **Ensure Chrome is installed** on your system. Adjust the path in the `SeleniumAuthenticator` class if needed.

## Usage

1. Open the `Program.cs` file and modify the `item` variable with the desired URL to convert to PDF:
   ```csharp
   var item = new Item
   {
       Name = "Sample Form",
       Href = "https://your-target-url.com"
   };
   ```

2. Build and run the application:
   ```bash
   dotnet run
   ```

3. Upon successful execution, a PDF file will be generated in the project's root directory.

## Code Structure

- **Program.cs**: The main entry point of the application that handles the PDF generation logic.
- **Item**: A simple model representing the item with a name and URL.
- **SeleniumAuthenticator**: A class responsible for retrieving the installed browser path.
- **Log**: A simple logging utility for displaying messages in the console.
- **LogLevel**: Enum for logging levels.

## Error Handling

The application includes a retry mechanism that attempts to download the PDF multiple times in case of service unavailability (HTTP 503). The retry count can be configured in the `DownloadFormToPdf` method.

## Customization

You can customize the PDF generation options in the `DownloadFormToPdf` method:
- **PrintBackground**: Include background graphics.
- **Format**: Page format (e.g., A4).
- **MarginOptions**: Set page margins (top, bottom, left, right).
- **Scale**: Scale factor for the PDF output.

## License

This project is licensed under the MIT License.

---

Feel free to reach out with any questions or issues!
