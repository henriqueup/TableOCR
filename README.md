# TableOCR
C# Visual Studio project for extracting information from tables using OCR.

The basic idea is to use HoughTransform to detect lines, which will delimit the table cells, then run OCR on each cell
and generate a CSV file with the output.
To do this, the first step is to load an Image. This can be done with a local file or using the built in screenshot feature.
Then, the Image is redimensioned and a binarization filter is applied, with a user inputted tolerance parameter,
so that the OCR can have better results.
The next step is to use the HoughTransform and detect the lines. To do this, the user must also input a tolerance for the
HoughTransform algorithm.
Then, the user can select line segments to delete until the delimited ones are compatible with the loaded image.
Finally, the TesseractOCR can be called and it wil execute for each delimited cell, generating the corresponding CSV.
