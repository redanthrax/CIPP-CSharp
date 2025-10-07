using Microsoft.JSInterop;
using MudBlazor;

namespace CIPP.Frontend.Services;

public interface IThemeService {
    MudTheme CurrentTheme { get; }
    bool IsDarkMode { get; }
    event Action? OnThemeChanged;
    Task InitializeAsync();
    Task ToggleThemeAsync();
    Task SetThemeAsync(bool isDarkMode);
}

public class ThemeService : IThemeService {
    private readonly IJSRuntime _jsRuntime;
    private bool _isDarkMode = false;
    private MudTheme _currentTheme = new();

    public MudTheme CurrentTheme => _currentTheme;
    public bool IsDarkMode => _isDarkMode;
    public event Action? OnThemeChanged;

    public ThemeService(IJSRuntime jsRuntime) {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync() {
        try {
            var storedTheme = await _jsRuntime.InvokeAsync<string?>("localStorageHelper.getItem", "cipp-theme");
            _isDarkMode = storedTheme == "dark";
            _currentTheme = _isDarkMode ? GetDarkTheme() : GetLightTheme();
        }
        catch {
            _isDarkMode = false;
            _currentTheme = GetLightTheme();
        }
        await UpdateThemeAttributeAsync();
        OnThemeChanged?.Invoke();
    }

    public async Task ToggleThemeAsync() {
        _isDarkMode = !_isDarkMode;
        _currentTheme = _isDarkMode ? GetDarkTheme() : GetLightTheme();
        
        try {
            var themeValue = _isDarkMode ? "dark" : "light";
            await _jsRuntime.InvokeVoidAsync("localStorageHelper.setItem", "cipp-theme", themeValue);
        }
        catch { }
        
        await UpdateThemeAttributeAsync();
        OnThemeChanged?.Invoke();
    }

    public async Task SetThemeAsync(bool isDarkMode) {
        if (_isDarkMode == isDarkMode) return;
        
        _isDarkMode = isDarkMode;
        _currentTheme = _isDarkMode ? GetDarkTheme() : GetLightTheme();
        
        try {
            var themeValue = _isDarkMode ? "dark" : "light";
            await _jsRuntime.InvokeVoidAsync("localStorageHelper.setItem", "cipp-theme", themeValue);
        }
        catch { }
        
        await UpdateThemeAttributeAsync();
        OnThemeChanged?.Invoke();
    }

    private async Task UpdateThemeAttributeAsync() {
        try {
            var themeValue = _isDarkMode ? "dark" : "light";
            await _jsRuntime.InvokeVoidAsync("localStorageHelper.setThemeAttribute", themeValue);
        }
        catch { }
    }

    private MudTheme GetLightTheme() => new MudTheme {
        PaletteLight = new PaletteLight {
            Black = "#110e00",
            Background = "#ffffff",
            BackgroundGray = "#f5f5f5",
            Surface = "#ffffff",
            DrawerBackground = "#ffffff",
            DrawerText = "rgba(0,0,0, 0.87)",
            AppbarBackground = "#f77f00",
            AppbarText = "#ffffff",
            TextPrimary = "rgba(0,0,0, 0.87)",
            TextSecondary = "rgba(0,0,0, 0.60)",
            ActionDefault = "#adadb1",
            ActionDisabled = "rgba(0,0,0, 0.26)",
            ActionDisabledBackground = "rgba(0,0,0, 0.12)",
            Primary = "#f77f00",
            PrimaryContrastText = "#ffffff",
            Secondary = "#fcbf49",
            SecondaryContrastText = "#000000",
            Tertiary = "#f9844a",
            TertiaryContrastText = "#ffffff",
            Info = "#2196f3",
            Success = "#4caf50",
            Warning = "#ff9800",
            Error = "#f44336",
            Dark = "#424242"
        }
    };

    private MudTheme GetDarkTheme() => new MudTheme {
        PaletteLight = new PaletteLight {
            Black = "#1a1a1a",
            Background = "#212121",
            BackgroundGray = "#2e2e2e",
            Surface = "#2e2e2e",
            DrawerBackground = "#1a1a1a",
            DrawerText = "rgba(255,255,255, 0.70)",
            AppbarBackground = "#1a1a1a",
            AppbarText = "rgba(255,255,255, 0.87)",
            TextPrimary = "rgba(255,255,255, 0.87)",
            TextSecondary = "rgba(255,255,255, 0.60)",
            ActionDefault = "#ffffff",
            ActionDisabled = "rgba(255,255,255, 0.26)",
            ActionDisabledBackground = "rgba(255,255,255, 0.12)",
            Primary = "#f77f00",
            PrimaryContrastText = "#ffffff",
            Secondary = "#fcbf49",
            SecondaryContrastText = "#000000",
            Tertiary = "#f9844a",
            TertiaryContrastText = "#ffffff",
            Info = "#2196f3",
            Success = "#4caf50",
            Warning = "#ff9800",
            Error = "#f44336",
            Dark = "#424242"
        }
    };
}
