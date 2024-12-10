using MauiPlanets.Models;

namespace Views;

public partial class PlanetsDetailsPage : ContentPage
{
	public PlanetsDetailsPage(Planet planet)
	{
		InitializeComponent();

		this.BindingContext = planet;
	}

	async void BackButton_Clicked(System.Object sender, System.EventArgs e)
	{
		await Navigation.PopAsync();
	}
}