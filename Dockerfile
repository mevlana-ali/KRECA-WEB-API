# Aşama 1: Build (Derleme)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Bütün katmanların csproj dosyalarını kendi klasör yapılarına tam uygun kopyalıyoruz
COPY ["Kreca Web API/Kreca Web API.csproj", "Kreca Web API/"]
COPY ["KReca.Business/KReca.Business.csproj", "KReca.Business/"]
COPY ["KReca.Data/KReca.Data.csproj", "KReca.Data/"]

# Ana API projesini restore ettiğimizde bağlı olduğu Data ve Business katmanları da otomatik yüklenir
RUN dotnet restore "Kreca Web API/Kreca Web API.csproj"

# Kalan tüm projeyi (klasörlerin içindekiler dahil) kopyala
COPY . .

# Build ve Publish işlemlerini API projesinin içine girerek yapıyoruz
WORKDIR "/src/Kreca Web API"
RUN dotnet publish "Kreca Web API.csproj" -c Release -o /app/publish

# Aşama 2: Run (Çalıştırma)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Render'ın varsayılan portunu dinlemesini sağlıyoruz
ENV ASPNETCORE_URLS=http://+:80

# DLL isminde boşluk olduğu için tam adını tırnak içinde yazıyoruz
ENTRYPOINT ["dotnet", "Kreca Web API.dll"]