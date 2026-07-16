using System.ComponentModel.DataAnnotations;

namespace KReca.Business.DTOs;

public class IletisimMesajiDto
{
    public int Id { get; set; }
    public string AdSoyad { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public string Mesaj { get; set; } = string.Empty;
    public bool Okundu { get; set; }
    public DateTime OlusturulmaTarihi { get; set; }
}

public class IletisimMesajiOlusturDto
{
    [Required(ErrorMessage = "Ad Soyad zorunludur")]
    [MaxLength(100)]
    public string AdSoyad { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta girin")]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon zorunludur")]
    [Phone]
    [MaxLength(20)]
    public string Telefon { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mesaj zorunludur")]
    [MinLength(10, ErrorMessage = "Mesaj en az 10 karakter olmalı")]
    [MaxLength(2000)]
    public string Mesaj { get; set; } = string.Empty;
}
