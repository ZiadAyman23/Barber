namespace Core.DTO;

public class ChangePassword
{
    [Required(ErrorMessage = "يرجى إدخال كلمة المرور الحالية")]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "يرجى إدخال كلمة المرور الجديدة")]
    [MinLength(6, ErrorMessage = "يجب ألا تقل كلمة المرور الجديدة عن 6 أحرف")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "يرجى تأكيد كلمة المرور الجديدة")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
    public string ConfirmPassword { get; set; } = string.Empty;
}