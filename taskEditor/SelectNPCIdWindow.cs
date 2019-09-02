﻿using JHUI.Forms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace sTASKedit
{
	public partial class SelectNPCIdWindow: JForm
    {
        int Param;
        bool Scrolling = false;
        private TaskEditor MainWindow;
        public SelectNPCIdWindow(TaskEditor MainWindow, int param)
		{
            InitializeComponent(); 
            this.MainWindow = MainWindow;
            Param = param;
            this.SetLocalization();

		}

        private void SelectNPCIdWindow_Load(object sender, EventArgs e)
        {
            this.dataGridView_NPCs.Rows.Clear();
            string[] values = new string[]
			    {
			        "0",
			        Extensions.GetLocalization(402)
			    };
            this.dataGridView_NPCs.Rows.Add(values);
            int l = 57;
            if(TaskEditor.eLC == null)
            {
                return;
            }
            if (l != TaskEditor.eLC.ConversationListIndex)
            {
                int pos = -1;
                for (int i = 0; i < TaskEditor.eLC.Lists[l].elementFields.Length; i++)
                {
                    if (TaskEditor.eLC.Lists[l].elementFields[i] == "Name")
                    {
                        pos = i;
                        break;
                    }
                }
                for (int k = 0; k < TaskEditor.eLC.Lists[l].elementValues.Count; k++)
                {
                    if (TaskEditor.eLC.Lists[l].elementFields[0] == "ID")
                    {
                        string[] strArray14 = new string[]
					        {
						        TaskEditor.eLC.GetValue(l, k, 0),
						        TaskEditor.eLC.GetValue(l, k, pos)
					        };
                        this.dataGridView_NPCs.Rows.Add(strArray14);
                    }
                }
            }
            this.dataGridView_NPCs.CurrentCell = this.dataGridView_NPCs.Rows[MainWindow.SelectNPCIdWindow_SelectedItemIndex].Cells[0];
            this.dataGridView_NPCs_SelectionChanged(null, null);
        }

        private void dataGridView_NPCs_SelectionChanged(object sender, EventArgs e)
        {
            int l = 57;
            int k = this.dataGridView_NPCs.CurrentCell.RowIndex - 1;
            int scroll = this.dataGridView_Props.FirstDisplayedScrollingRowIndex;
            this.dataGridView_Props.Rows.Clear();
            if (k >= 0)
            {
                for (int f = 0; f < TaskEditor.eLC.Lists[l].elementValues[k].Count; f++)
                {
                    this.dataGridView_Props.Rows.Add(new string[] { TaskEditor.eLC.Lists[l].elementFields[f], TaskEditor.eLC.GetValue(l, k, f) });
                }
            }
            if (scroll > -1 && k > -1)
            {
                this.dataGridView_Props.FirstDisplayedScrollingRowIndex = scroll;
            }
        }

        private void dataGridView_NPCs_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (Scrolling == false && e.ColumnIndex >= 0 && e.ColumnIndex < 2 && e.RowIndex > -1)
            {
                String text = "";
                try
                {
                    int Id = Convert.ToInt32(this.dataGridView_NPCs.Rows[e.RowIndex].Cells[0].Value);
                    if (Id > 0 && MainWindow.lEnableShowInfo == true)
                    {
                        if (TaskEditor.LoadedElements == true) text += Extensions.ColorClean(Extensions.GetMonsterNPCMineProps(Id));
                    }
                    this.dataGridView_NPCs.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = text;
                }
                catch
                {
                    this.dataGridView_NPCs.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = text;
                }
            }
        }

        private void dataGridView_NPCs_Scroll(object sender, ScrollEventArgs e)
        {
            Scrolling = true;
        }

        private void dataGridView_NPCs_MouseMove(object sender, MouseEventArgs e)
        {
            if (Scrolling == true)
            {
                Scrolling = false;
            }
        }

        private void textBox_SearchNPCId_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i <= this.dataGridView_NPCs.Rows.Count - 1; i++)
            {
                if (this.dataGridView_NPCs.Rows[i].Cells[0].FormattedValue.ToString() == textBox_SearchNPCId.Text)
                {
                    this.dataGridView_NPCs.CurrentCell = this.dataGridView_NPCs.Rows[i].Cells[0];
                    this.dataGridView_NPCs_SelectionChanged(null, null);
                    break;
                }
            }
        }

        private void textBox_SearchNPCName_TextChanged(object sender, EventArgs e)
        {
            int k = 0;
            int index = this.dataGridView_NPCs.CurrentCell.RowIndex;
            for (int i = index; i <= this.dataGridView_NPCs.Rows.Count - 1; i++)
            {
                if (this.dataGridView_NPCs.Rows[i].Cells[1].FormattedValue.ToString().ToLower().Contains(textBox_SearchNPCName.Text.ToLower()))
                {
                    this.dataGridView_NPCs.CurrentCell = this.dataGridView_NPCs.Rows[i].Cells[1];
                    this.dataGridView_NPCs_SelectionChanged(null, null);
                    break;
                }
                if (i == this.dataGridView_NPCs.Rows.Count - 1 && k == 0)
                {
                    k = 1;
                    i = 0;
                }
            }
        }

        private void textBox_SearchNPCId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.textBox_SearchNPCId_TextChanged(null, null);
            }
        }

        private void textBox_SearchNPCName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                int k = 0;
                int index = this.dataGridView_NPCs.CurrentCell.RowIndex + 1;
                for (int i = index; i <= this.dataGridView_NPCs.Rows.Count - 1; i++)
                {
                    if (this.dataGridView_NPCs.Rows[i].Cells[1].FormattedValue.ToString().ToLower().Contains(textBox_SearchNPCName.Text.ToLower()))
                    {
                        this.dataGridView_NPCs.CurrentCell = this.dataGridView_NPCs.Rows[i].Cells[1];
                        this.dataGridView_NPCs_SelectionChanged(null, null);
                        break;
                    }
                    if (i == this.dataGridView_NPCs.Rows.Count - 1 && k == 0)
                    {
                        k = 1;
                        i = 0;
                    }
                }
            }
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            MainWindow.ChangeNPCId(Convert.ToInt32(this.dataGridView_NPCs.CurrentRow.Cells[0].Value), Param);
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SelectNPCIdWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainWindow.SelectNPCIdWindow_SelectedItemIndex = this.dataGridView_NPCs.CurrentCell.RowIndex;
        }

        private void SelectNPCIdWindow_SizeChanged(object sender, EventArgs e)
        {
           // if (this.WindowState != FormWindowState.Maximized) MainWindow.lSelectNPCIdWindow_Size = this.Size;
        }

        public void SetLocalization()
        {
            Text = Extensions.GetLocalization(6041);
            Column_NPCId.HeaderText = Extensions.GetLocalization(6042);
            Column_NPCName.HeaderText = Extensions.GetLocalization(6025);
            Column_Parameter.HeaderText = Extensions.GetLocalization(6049);
            Column_Value.HeaderText = Extensions.GetLocalization(6050);
            label1.Text = Extensions.GetLocalization(6026);
            label2.Text = Extensions.GetLocalization(6027);
            button_Ok.Text = Extensions.GetLocalization(6001);
            button_Cancel.Text = Extensions.GetLocalization(6002);
        }
	}
}
