using Godot;

namespace NewArcana.Scripts;

public partial class TimeManager : Node
{
	private int _currentMonth = 0;
	private int _currentYear = 1;

	private float _timePassed = 0.0f;
	private const float TimeToAdvanceMonth = 1.0f;

	[Signal]
	public delegate void MonthPassedEventHandler(int month, int year);

	[Signal]
	public delegate void YearPassedEventHandler(int year);

	public override void _Process(double delta)
	{
		_timePassed += (float)delta;

		if (_timePassed >= TimeToAdvanceMonth)
		{
			_timePassed -= TimeToAdvanceMonth;
			OnMonthPassed();
		}
	}

	private void OnMonthPassed()
	{
		_currentMonth++;
		if (_currentMonth > 12)
		{
			_currentMonth = 1;
			_currentYear++;
			EmitSignal(SignalName.YearPassed, _currentYear);
		}

		EmitSignal(SignalName.MonthPassed, _currentMonth, _currentYear);
	}

	public int GetCurrentMonth() => _currentMonth;
	public int GetCurrentYear() => _currentYear;
}
