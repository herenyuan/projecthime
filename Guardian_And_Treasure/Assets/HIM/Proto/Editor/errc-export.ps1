param (
    [string] $src,
    [string] $dst
)

function process([string]$src, [string]$excelpath, $xl) {
    $wb = $xl.Workbooks.Add()
    $sheet = $wb.WorkSheets.item(1)

    $str = Get-Content -encoding UTF8 $src

    $pattern = "(?<Name>\w+) *= *(?<Code>\d+) *; *//(?<Description>\w+)"

    #"{`"`":[" | Out-File -NoNewline -FilePath $dst -Encoding utf8

    $i = 1

    $sheet.cells.item($i, 2) = "//Name"
    $sheet.cells.item($i, 1) = "ID"
    $sheet.cells.item($i, 3) = "Description"
    $i++

    $sheet.cells.item($i, 2) = "string"
    $sheet.cells.item($i, 1) = "int"
    $sheet.cells.item($i, 3) = "string"
    $i++

    [regex]::Matches($str, $pattern) | ForEach-Object {
		"处理$_"
    <#
@"
    {
        "name": "$($_.groups[1].value)",
        "code": $($_.groups[2].value),
        "description": "$($_.groups[3].value)"
    },
"@ | Out-File -NoNewline -Append -FilePath $dst -Encoding utf8
        $_.captures.groups["Code"].value
    #>

        $sheet.cells.item($i, 2) = $($_.groups[1].value)
        $sheet.cells.item($i, 1) = $($_.groups[2].value)
        $sheet.cells.item($i, 3) = $($_.groups[3].value)

        $i++
    }

    #"`n]}" | Out-File -Append -FilePath $dst -Encoding utf8

    $wb.SaveAs($excelpath)
    $xl.Workbooks.Close()
}

$xl = New-Object -ComObject Excel.Application

process -src $src -excelpath "$([System.IO.Path]::GetFullPath($dst))" -xl $xl

$xl.quit()