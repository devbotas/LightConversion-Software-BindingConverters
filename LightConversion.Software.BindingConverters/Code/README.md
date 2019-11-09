# QuickConverter

## Examples

Boolean property:
```XML
<CheckBox IsEnabled="{qc:Binding '!$P', P={Binding Path=ViewModel.SomeBooleanProperty}}" />
```

Visibility:
```XML
<CheckBox Visibility="{qc:Binding '$P ? Visibility.Visible : Visibility.Collapsed', P={Binding ShowElement}}" />
```


Doing a null check:
```XML
<CheckBox IsEnabled="{qc:Binding '$P != null', P={Binding Path=SomeProperty}" />
```

Checking a class instance’s property values:
```XML
<CheckBox IsEnabled="{qc:Binding '$P.IsValid || $P.ForceAlways', P={Binding Path=SomeClassInstance}" />
```


Doing two-way binding:
```XML
<CheckBox Height="{qc:Binding '$P * 10', ConvertBack='$value * 0.1', P={Binding TestWidth, Mode=TwoWay}}" />
```

Doing Multi-binding:
```XML
<CheckBox Angle="{qc:MultiBinding 'Math.Atan2($P0, $P1) * 180 / 3.14159', P0={Binding ActualHeight, ElementName=rootElement}, P1={Binding ActualWidth, ElementName=rootElement}}" />
```

Declaring and using local variables in your converter expression:
```XML
<CheckBox IsEnabled="{qc:Binding '(Loc = $P.Value, A = $P.Show) => $Loc != null &amp;&amp; $A', P={Binding Obj}}" />
```

* Note that the "&&" operator must be written as ```&amp;&amp;``` in XML.

And there is even limited support for using lambdas, which allows LINQ to be used:
```XML
<ItemsControl ItemsSource="{qc:Binding '$P.Where(( (int)i ) => (bool)($i % 2 == 0))', P={Binding Source}}" />
```

## Register assemblies for the types that you plan to use in your quick converters

```C#
public partial class App : Application {
    public App() : base() {
        // Setup Quick Converter.
        QuickConverter.EquationTokenizer.AddNamespace(typeof(My.Namespace));
    }
}
```