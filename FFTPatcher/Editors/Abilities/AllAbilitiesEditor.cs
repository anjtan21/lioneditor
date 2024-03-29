/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class AllAbilitiesEditor : UserControl
    {
		#region Instance Variables (6) 

        private Ability cbAbility;
        const int cloneCommonIndex = 0;
        private Context ourContext = Context.Default;
        const int pasteAllIndex = 3;
        const int pasteCommonIndex = 1;
        const int pasteSpecificIndex = 2;

		#endregion Instance Variables 

		#region Constructors (1) 

        public AllAbilitiesEditor()
        {
            InitializeComponent();
            abilityEditor.InflictStatusLabelClicked += abilityEditor_InflictStatusLabelClicked;
            abilityEditor.DataChanged += new EventHandler( abilityEditor_DataChanged );
            abilitiesListBox.MouseDown += new MouseEventHandler( abilitiesListBox_MouseDown );
            abilitiesListBox.ContextMenu = new ContextMenu( new MenuItem[] {
                new MenuItem("Clone", copyAll),
                new MenuItem("Paste Common", pasteCommon),
                new MenuItem("Paste XXX", pasteSpecific),
                new MenuItem("Paste All", pasteAll) } );
            abilitiesListBox.ContextMenu.Popup += new EventHandler( ContextMenu_Popup );
        }

		#endregion Constructors 

		#region Public Methods (2) 

        public void abilitiesListBox_MouseDown( object sender, MouseEventArgs e )
        {
            if( e.Button == MouseButtons.Right )
            {
                abilitiesListBox.SelectedIndex = abilitiesListBox.IndexFromPoint( e.Location );
            }
        }

        public void UpdateView( AllAbilities allAbilities )
        {
            if( ourContext != FFTPatch.Context )
            {
                ourContext = FFTPatch.Context;
                cbAbility = null;
            }
            abilitiesListBox.SelectedIndexChanged -= abilitiesListBox_SelectedIndexChanged;
            abilitiesListBox.DataSource = allAbilities.Abilities;
            abilitiesListBox.SelectedIndexChanged += abilitiesListBox_SelectedIndexChanged;
            abilitiesListBox.SelectedIndex = 0;
            abilityEditor.Ability = abilitiesListBox.SelectedItem as Ability;
        }

		#endregion Public Methods 

		#region Private Methods (9) 

        private void abilitiesListBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            Ability a = abilitiesListBox.SelectedItem as Ability;
            abilityEditor.Ability = a;
        }

        private void abilityEditor_DataChanged( object sender, EventArgs e )
        {
            CurrencyManager cm = (CurrencyManager)BindingContext[abilitiesListBox.DataSource];
            cm.Refresh();
        }

        private void abilityEditor_InflictStatusLabelClicked( object sender, LabelClickedEventArgs e )
        {
            if( InflictStatusClicked != null )
            {
                InflictStatusClicked( this, e );
            }
        }

private void ContextMenu_Popup( object sender, EventArgs e )
        {
            AbType cbType =
                cbAbility == null ? AbType.None :
                cbAbility.IsArithmetick ? AbType.Arithmetick :
                cbAbility.IsCharging ? AbType.Charging :
                cbAbility.IsItem ? AbType.Item :
                cbAbility.IsJumping ? AbType.Jumping :
                cbAbility.IsNormal ? AbType.Normal :
                cbAbility.IsOther ? AbType.Other :
                                    AbType.Throwing;
            bool typesMatch = TypesMatch();

            abilitiesListBox.ContextMenu.MenuItems[pasteCommonIndex].Enabled = cbType != AbType.None;
            abilitiesListBox.ContextMenu.MenuItems[pasteAllIndex].Enabled = typesMatch;
            abilitiesListBox.ContextMenu.MenuItems[pasteSpecificIndex].Enabled = typesMatch;
            abilitiesListBox.ContextMenu.MenuItems[pasteSpecificIndex].Text = string.Format( "Paste {0}", cbType );

        }

        private void copyAll( object sender, EventArgs args )
        {
            cbAbility = abilitiesListBox.SelectedItem as Ability;
        }

        private void pasteAll( object sender, EventArgs args )
        {
            if( TypesMatch() )
            {
                cbAbility.CopyAllTo( abilitiesListBox.SelectedItem as Ability );
                abilityEditor.UpdateView();
                abilityEditor_DataChanged( abilityEditor, EventArgs.Empty );
            }
        }

        private void pasteCommon( object sender, EventArgs args )
        {
            if( cbAbility != null )
            {
                Ability destAbility = abilitiesListBox.SelectedItem as Ability;
                cbAbility.CopyCommonTo( destAbility );
                abilityEditor.UpdateView();
                abilityEditor_DataChanged( abilityEditor, EventArgs.Empty );
            }
        }

        private void pasteSpecific( object sender, EventArgs args )
        {
            if( TypesMatch() )
            {
                cbAbility.CopySpecificTo( abilitiesListBox.SelectedItem as Ability );
                abilityEditor.UpdateView();
                abilityEditor_DataChanged( abilityEditor, EventArgs.Empty );
            }
        }

        private bool TypesMatch()
        {
            Ability destinationAbility = abilitiesListBox.SelectedItem as Ability;
            AbType cbType =
                cbAbility == null ? AbType.None :
                cbAbility.IsArithmetick ? AbType.Arithmetick :
                cbAbility.IsCharging ? AbType.Charging :
                cbAbility.IsItem ? AbType.Item :
                cbAbility.IsJumping ? AbType.Jumping :
                cbAbility.IsNormal ? AbType.Normal :
                cbAbility.IsOther ? AbType.Other :
                                    AbType.Throwing;
            AbType destType =
                destinationAbility == null ? AbType.None :
                destinationAbility.IsArithmetick ? AbType.Arithmetick :
                destinationAbility.IsCharging ? AbType.Charging :
                destinationAbility.IsItem ? AbType.Item :
                destinationAbility.IsJumping ? AbType.Jumping :
                destinationAbility.IsNormal ? AbType.Normal :
                destinationAbility.IsOther ? AbType.Other :
                                            AbType.Throwing;
            return cbType != AbType.None && destType != AbType.None && cbType == destType;
        }

		#endregion Private Methods 

        private enum AbType
        {
            None,
            Arithmetick,
            Charging,
            Item,
            Jumping,
            Normal,
            Other,
            Throwing
        }
public event EventHandler<LabelClickedEventArgs> InflictStatusClicked;
    }
}
