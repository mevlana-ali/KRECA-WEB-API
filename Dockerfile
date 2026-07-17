# Aşama 1: Build (Derleme)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Sadece proje dosyasını kopyalayıp bağımlılıkları yüklüyoruz (Cache avantajı)
COPY *.csproj ./
RUN dotnet restore

# Tüm kodları kopyalayıp projeyi yayınlıyoruz
COPY . ./
RUN dotnet publish -c Release -o out

# Aşama 2: Run (Çalıştırma)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Render'ın varsayılan portunu dinlemesini sağlıyoruz
ENV ASPNETCORE_URLS=http://+:80

# DİKKAT: Aşağıdaki "ProjeAdin.dll" kısmını kendi projenin asıl adıyla değiştir!
ENTRYPOINT ["dotnet", "Kreca Web API.dll"]