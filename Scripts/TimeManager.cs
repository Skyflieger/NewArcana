using Godot;

namespace NewArcana.Scripts;

public partial class TimeManager : Node
{
	private int _currentMonth = 0;
	private int _currentYear = 1;

	private float _timePassed = 0.0f;
	private const float TimeToAdvanceMonth = 1.0f;
	public bool Paused = true;
	[Signal]
	public delegate void MonthPassedEventHandler(int month, int year);

	[Signal]
	public delegate void YearPassedEventHandler(int year);
	
	[Signal]
	public delegate void CityMonthPassedEventHandler(int month, int year);

	[Signal]
	public delegate void CityYearPassedEventHandler(int year);
	[Signal]
	public delegate void BaseMonthPassedEventHandler(int month, int year);

	[Signal]
	public delegate void BaseYearPassedEventHandler(int year);
	[Signal]
	public delegate void FarmMonthPassedEventHandler(int month, int year);

	[Signal]
	public delegate void FarmYearPassedEventHandler(int year);

	public override void _Process(double delta)
	{

		if(Paused)
			return;
		_timePassed += (float)delta;

		if (_timePassed >= TimeToAdvanceMonth)
		{
			_timePassed -= TimeToAdvanceMonth;
			OnMonthPassed();
		}
	}

	public void Pause()
	{
		Paused = true;
	}

	public void Resume()
	{
		Paused = false;
	}

	private void OnMonthPassed()
	{
		_currentMonth++;
		EmitSignal(SignalName.BaseMonthPassed, _currentMonth, _currentYear);
		EmitSignal(SignalName.FarmMonthPassed, _currentMonth, _currentYear);

		EmitSignal(SignalName.CityMonthPassed, _currentMonth, _currentYear);
		EmitSignal(SignalName.MonthPassed, _currentMonth, _currentYear);
		if (_currentMonth > 12)
		{
			_currentMonth = 1;
			_currentYear++;
			EmitSignal(SignalName.BaseYearPassed, _currentYear);
			EmitSignal(SignalName.FarmYearPassed, _currentYear);

			EmitSignal(SignalName.CityYearPassed, _currentYear);
			EmitSignal(SignalName.YearPassed, _currentYear);
		}
	}

	public int GetCurrentMonth() => _currentMonth;
	public int GetCurrentYear() => _currentYear;
}
