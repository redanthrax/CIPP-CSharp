using Microsoft.JSInterop;
using MudBlazor;

namespace CIPP.Frontend.WASM.Services;

public interface IThemeService {
    MudTheme CurrentTheme { get; }
    bool IsDarkMode { get; }
    event Action? OnThemeChanged;
    Task InitializeAsync();
    Task ToggleThemeAsync();
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
            _isDarkMode = await _jsRuntime.InvokeAsync<bool>("localStorage.getItem", "darkMode");
            _currentTheme = _isDarkMode ? GetDarkTheme() : new MudTheme();
        }
        catch {
            _isDarkMode = false;
            _currentTheme = new MudTheme();
        }
        OnThemeChanged?.Invoke();
    }

    public async Task ToggleThemeAsync() {
        _isDarkMode = !_isDarkMode;
        _currentTheme = _isDarkMode ? GetDarkTheme() : new MudTheme();
        
        try {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "darkMode", _isDarkMode);
        }
        catch {
            // Ignore if localStorage isn't available
        }
        
        OnThemeChanged?.Invoke();
    }

    private MudTheme GetDarkTheme() => new MudTheme {
        PaletteLight = new PaletteLight {
            Black = "#27272f",
            Background = "#32333d",
            BackgroundGray = "#27272f",
            Surface = "#373740",
            DrawerBackground = "#27272f",
            DrawerText = "rgba(255,255,255, 0.50)",
            AppbarBackground = "#373740",
            AppbarText = "rgba(255,255,255, 0.70)",
            TextPrimary = "rgba(255,255,255, 0.70)",
            TextSecondary = "rgba(255,255,255, 0.50)",
            ActionDefault = "#adadb1",
            ActionDisabled = "rgba(255,255,255, 0.26)",
            ActionDisabledBackground = "rgba(255,255,255, 0.12)"
        }
    };
}