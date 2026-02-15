[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$baseDir = $PSScriptRoot

Write-Host "Starting ETicaret Project Components..." -ForegroundColor Green

# Start RestApi
Write-Host "Starting Rest Api (http://localhost:5292)..."
Start-Process powershell -ArgumentList "-NoExit", "-Command", "chcp 65001 | Out-Null; Set-Location '$baseDir'; dotnet run --project RestApi/ETicaretAPI/ETicaretAPI.csproj --urls=http://localhost:5292"

# Start AdminPanel
Write-Host "Starting Admin Panel (http://localhost:5012)..."
Start-Process powershell -ArgumentList "-NoExit", "-Command", "chcp 65001 | Out-Null; Set-Location '$baseDir'; dotnet run --project AdminPanel/ETicaretMVC/ETicaretMVC.csproj --urls=http://localhost:5012"

# Start Frontend
Write-Host "Starting Frontend (http://localhost:4200)..."
Start-Process powershell -ArgumentList "-NoExit", "-Command", "chcp 65001 | Out-Null; Set-Location '$baseDir\FrontEnd'; npm start"

Write-Host "All components launched in separate PowerShell windows." -ForegroundColor Green
