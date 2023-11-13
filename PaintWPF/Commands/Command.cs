using PaintWPF.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PaintWPF.Models;
internal class RelayCommand : CommandBase
{
    private readonly Action _executeAction;

    public RelayCommand(Action executeAction)
    {
        _executeAction = executeAction;
    }

    public override void Execute(object parameter) =>
        _executeAction();
}