using MauiPlanets.Models;
using Services;

namespace Views;

public partial class PlanetsPage : ContentPage
{
    private const uint AnimationDuration = 800u;
    public PlanetsPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
		base.OnAppearing();

		lstPopularPlanets.ItemsSource = PlanetsService.GetFeaturedPlanets();
		lstAllPlanets.ItemsSource = PlanetsService.GetAllPlanets();
    }

	async void Planets_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
	{
		await Navigation.PushAsync(new PlanetsDetailsPage(e.CurrentSelection.First() as Planet));
	}

    async void GridArea_Tapped(System.Object sender, System.EventArgs e)
	{
		await CloseMenu();
	}

	private async Task CloseMenu()
	{
        _ = MainContentGrid.FadeTo(1, AnimationDuration);
        _ = MainContentGrid.ScaleTo(1, AnimationDuration);
        await MainContentGrid.TranslateTo(0, 0, AnimationDuration, Easing.CubicIn);
    }

	async void ProfilePic_Clicked(System.Object sender, System.EventArgs e)
	{
		_ = MainContentGrid.TranslateTo(-this.Width * 0.5, this.Height * 0.1, AnimationDuration, Easing.CubicIn);
		await MainContentGrid.ScaleTo(0.8, AnimationDuration);
		_ = MainContentGrid.ScaleTo(0.8, AnimationDuration);
	}
}