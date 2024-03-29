using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MultiCamLibrary.Property
{
    public abstract class PropertyBase : INotifyPropertyChanged
    {
        

        public abstract string Name { get; }

        public abstract string DisplayName { get; }

        public virtual PropertyRecord GetPropertyRecord(string ownerName)
        {
            return new PropertyRecord { Name = this.Name, Value = "" };
            //return new PropertyRecord { Name = $"{ownerName}_{this.Name}", Value = ""};
        }

        public virtual void SetByPropertyRecord(PropertyRecord record)
        {

        }

        protected bool _isEnabled = true;
        virtual public bool IsEnabled { get => _isEnabled; set { _isEnabled = value; OnPropertyChanged(); } }

        public virtual void Update() { }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public virtual string GetStringValue()
        {
            return "GetStringValue undefined";
        }
    }
    public abstract class Property<T> : PropertyBase, IIncremented<T>
    {
        protected T min_value;

        protected T max_value;


        public T MinValue { get { return min_value; } }
        public T MaxValue { get { return max_value; } }


        protected T value;
        public T Value
        {
            get
            {

                var ret_value = GetValue();
                return ret_value;
            }
            set
            {
                if (IsEnabled == false) return;
                this.value = value;
                SetValue(value);
                OnPropertyChanged();
            }
        }

        public virtual T Increment { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual T MinIncrement => throw new NotImplementedException();

        public virtual void SetValue(T val)
        {

        }

        public virtual T GetValue()
        {
            return value;
        }

        public override PropertyRecord GetPropertyRecord(string ownerName)
        {
            return new PropertyRecord { Name = this.Name, Value = this.Value.ToString() };
        }

        public override void SetByPropertyRecord(PropertyRecord record)
        {
            Value = (T)Convert.ChangeType(record.Value, typeof(T));
            Update();
        }

        public override void Update()
        {
            var oldEnabled = IsEnabled;
            // IsEnabled = true;
            value = GetValue();
            OnPropertyChanged("Value");
            //IsEnabled = oldEnabled;
        }

        public override string GetStringValue()
        {
            return Value.ToString();
        }
    }

    interface IIncremented<T>
    {
        T Increment { get; set; }
        T MinIncrement { get; }
    }

    public class DoubleProperty : Property<double>
    {
        private string _name;
        public override string Name => _name;

        private string _displayName;
        public override string DisplayName => _displayName;
        public DoubleProperty(string name, string displayName)
        {
            _name = name;
            _displayName = displayName;
        }

        protected double increment;
        public override double Increment
        {
            get => increment;
            set
            {
                if (value < MinIncrement)
                {
                    increment = MinIncrement;
                }
                else
                {
                    increment = value;
                }
                OnPropertyChanged();
            }
        }

        public virtual string FormatString => "0.00";

        public override double MinIncrement { get; }

        
    }

    public abstract class BoolProperty : Property<bool>
    {

    }

    public abstract class uIntProperty : Property<uint>, IIncremented<uint>
    {
        private string _name;
        public override string Name => _name;

        private string _displayName;
        public override string DisplayName => _displayName;

        public uIntProperty(string name, string displayName)
        {
            _name = name;
            _displayName = displayName;
        }

        protected uint increment;
        public override uint Increment
        {
            get => increment;
            set
            {
                if (value < MinIncrement)
                {
                    increment = MinIncrement;

                }
                else
                {
                    increment = value;
                }
                OnPropertyChanged();
            }
        }

        public override uint MinIncrement { get; }

    }

    public abstract class IntProperty : Property<int>, IIncremented<int>
    {
        private string _name;
        public override string Name => _name;

        private string _displayName;
        public override string DisplayName => _displayName;

        public IntProperty(string name, string displayName)
        {
            _name = name;
            _displayName = displayName;
        }

        protected int increment;
        public int Increment
        {
            get => increment;
            set
            {
                if (value < MinIncrement)
                {
                    increment = MinIncrement;

                }
                else
                {
                    increment = value;
                }
                OnPropertyChanged();
            }
        }

        public abstract int MinIncrement { get; }

    }

    public abstract class ButtonPlusMinusProperty : Property<string>
    {
        public ButtonPlusMinusProperty()
        {
            value = "";
        }
        RelayCommand plusCommand;
        public RelayCommand PlusCommand
        {
            get
            {
                return plusCommand ?? (plusCommand = new RelayCommand(() =>
                {
                    PlusButton();
                    OnPropertyChanged("Value");
                }));
            }
        }

        RelayCommand minusCommand;
        public RelayCommand MinusCommand
        {
            get
            {
                return minusCommand ?? (minusCommand = new RelayCommand(() =>
                {
                    MinusButton();
                    OnPropertyChanged("Value");
                }));
            }
        }

        RelayCommand plusMouseDownCommand;

        public RelayCommand PlusMouseDownCommand
        {
            get
            {
                return plusMouseDownCommand ?? (plusMouseDownCommand = new RelayCommand(() =>
                {
                    PlusMouseDown();
                    OnPropertyChanged("Value");
                }));
            }
        }

        RelayCommand plusMouseUpCommand;

        public RelayCommand PlusMouseUpCommand
        {
            get
            {
                return plusMouseUpCommand ?? (plusMouseUpCommand = new RelayCommand(() =>
                {
                    PlusMouseUp();
                    OnPropertyChanged("Value");
                }));
            }
        }

        RelayCommand minusMouseDownCommand;

        public RelayCommand MinusMouseDownCommand
        {
            get
            {
                return minusMouseDownCommand ?? (minusMouseDownCommand = new RelayCommand(() =>
                {
                    MinusMouseDown();
                    OnPropertyChanged("Value");
                }));
            }
        }

        RelayCommand minusMouseUpCommand;

        public RelayCommand MinusMouseUpCommand
        {
            get
            {
                return minusMouseUpCommand ?? (minusMouseUpCommand = new RelayCommand(() =>
                {
                    MinusMouseUp();
                    OnPropertyChanged("Value");
                }));
            }
        }

        protected bool _isMinusEnabled = true;
        public bool IsMinusEnabled
        {
            get => _isMinusEnabled;
            set
            {
                _isMinusEnabled = value;
                OnPropertyChanged();
            }
        }

        protected bool _isPlusEnabled = true;
        public bool IsPlusEnabled
        {
            get => _isPlusEnabled;
            set
            {
                _isPlusEnabled = value;
                OnPropertyChanged();
            }
        }

        public virtual void PlusButton()
        {

        }
        public virtual void MinusButton()
        {

        }

        public virtual void PlusMouseDown()
        {

        }

        public virtual void PlusMouseUp()
        {

        }

        public virtual void MinusMouseDown()
        {

        }

        public virtual void MinusMouseUp()
        {

        }

        public override PropertyRecord GetPropertyRecord(string ownerName)
        {
            return null;
        }

    }

    public abstract class ListProperty : PropertyBase
    {

        protected ObservableCollection<string> values;
        public ObservableCollection<string> Values
        {
            get
            {
                var list = GetValues();
                return list;
            }
            protected set
            {
                values = value;
                OnPropertyChanged();
            }
        }

        protected int selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                SelectedItemChanged();
                OnPropertyChanged();
            }
        }

        protected virtual void SelectedItemChanged()
        {

        }

        protected virtual ObservableCollection<string> GetValues()
        {
            return values;
        }

        public ListProperty()
        {
            values = new ObservableCollection<string>();
        }

        public override PropertyRecord GetPropertyRecord(string ownerName)
        {
            return new PropertyRecord { Name = this.Name, Value = this.values[selectedIndex] };
        }

        public override void SetByPropertyRecord(PropertyRecord record)
        {
            var index = values.IndexOf(record.Value);
            SelectedIndex = index;
        }

        public override string GetStringValue()
        {
            return values[selectedIndex];
        }
    }

    public abstract class ButtonProperty : PropertyBase
    {
        private RelayCommand buttonCommand;
        public RelayCommand ButtonCommand
        {
            get
            {
                return buttonCommand ?? (buttonCommand = new RelayCommand(() =>
                {
                    OnButtonClicked();
                })); ;
            }
        }
        abstract public void OnButtonClicked();

        public override PropertyRecord GetPropertyRecord(string ownerName)
        {
            return null;
        }
    }
}
