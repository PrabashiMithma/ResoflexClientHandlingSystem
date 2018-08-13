﻿using MySql.Data.MySqlClient;
using ResoflexClientHandlingSystem.Core;
using ResoflexClientHandlingSystem.Role;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResoflexClientHandlingSystem
{
    public partial class ScheduleForm : MetroFramework.Forms.MetroForm
    {

        public ScheduleForm()
        {
            InitializeComponent();

            scheduleGrid.DataSource = getSchedules();

            //autocomplete data source
            projectName.AutoCompleteCustomSource = projectNameAutoComplete();
            clientName.AutoCompleteCustomSource = clientNameAutoComplete();
        }

        private void ScheduleForm_Load(object sender, EventArgs e)
        {
            
        }

        //buttons
        private void deleteSchedule_Click(object sender, EventArgs e)
        {

        }

        private void updateSchedule_Click(object sender, EventArgs e)
        {
            AddScheduleForm asf = new AddScheduleForm();

        }

        private void addSchedule_Click(object sender, EventArgs e)
        {
            AddScheduleForm asf = new AddScheduleForm();
            asf.ShowDialog();
        }

        private void schHome_Click(object sender, EventArgs e)
        {
            ProjectManager pm = new ProjectManager();
            this.Close();
            pm.Show();
        }

        private void addEvent_Click(object sender, EventArgs e)
        {
            AddEventForm ef = new AddEventForm();
            ef.ShowDialog();
        }

        //grid
        private void metroGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        //search by project name
        private void searchScheduleByProjectName_TextChanged(object sender, EventArgs e)
        {

            string projName = projectName.Text.ToString();

            string sql = "select s.sch_no as Schedule_No, p.proj_name as Project_Name, vt.type as Schedule_Type, s.from_date_time as Start_Date_and_Time, s.to_date_time as End_Date_and_Time, s.vehicle_details as Vehicle_Details, s.mileage as Mileage, s.to_do_list as TODO_List, s.resource as Resources, s.check_list as Check_List, s.travelling_mode as Travelling_Mode, s.accommodation as Accomodation, s.meals as Meals " +
                         " from schedule s, project p, visit_type vt " +
                         " where(s.proj_id = p.proj_id) and (s.visit_type_id = vt.visit_type_id) and (p.proj_name like '%" + projName + "%') " +
                         " order by s.sch_no, s.proj_id;";

            try
            {
                MySqlDataReader reader = DBConnection.getData(sql);

                if (reader.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    scheduleGrid.DataSource = dt;
                }
                else
                {
                    //scheduleGrid.DataSource = null;

                    reader.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //search by client name
        private void searchScheduleByClientName_TextChanged(object sender, EventArgs e)
        {

            string cName = clientName.Text.ToString();

            string sql = "select s.sch_no as Schedule_No, p.proj_name as Project_Name, vt.type as Schedule_Type, s.from_date_time as Start_Date_and_Time, s.to_date_time as End_Date_and_Time, s.vehicle_details as Vehicle_Details, s.mileage as Mileage, s.to_do_list as TODO_List, s.resource as Resources, s.check_list as Check_List, s.travelling_mode as Travelling_Mode, s.accommodation as Accomodation, s.meals as Meals " +
                         " from schedule s, project p, visit_type vt, client c " +
                         " where (s.proj_id = p.proj_id) and (s.visit_type_id = vt.visit_type_id) and (p.client_id = c.client_id) and (c.name like '%"+ cName + "%') " +
                         " order by s.sch_no, s.proj_id;";

            try
            {
                MySqlDataReader reader = DBConnection.getData(sql);

                if (reader.HasRows)
                { 
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    scheduleGrid.DataSource = dt;
                }
                else
                {
                    scheduleGrid.DataSource = null;

                    reader.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //getting all the relavent data for selected row
        private Schedule getScheduleRow(int schNo)
        {
            Schedule s = new Schedule();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            LinkedList<String> serviceEng = new LinkedList<String>();

            MySqlDataReader reader1 = DBConnection.getData("select * from schedule where sch_no =" + schNo + ";");
            MySqlDataReader reader2 = DBConnection.getData("select  from schedule_technicians where sch_no =" + schNo + ";");

            dt1.Load(reader1);
            dt2.Load(reader2);

            foreach (DataRow row in dt2.Rows)
            {

            }

            return null; //for now
        }


        private void updateSchedule_OnClick(object sender, EventArgs e)
        {
            DataGridViewRow row = scheduleGrid.CurrentRow;

            int sch_no = int.Parse((row.Cells[0].Value.ToString()));
            

            Schedule s = new Schedule();

        }

        //Data for grid
        private DataTable getSchedules()
        {
            DataTable dt = new DataTable();

            MySqlDataReader reader = DBConnection.getData("select s.sch_no as Schedule_No, p.proj_name as Project_Name, vt.type as Schedule_Type, s.from_date_time as Start_Date_and_Time, s.to_date_time as End_Date_and_Time, s.vehicle_details as Vehicle_Details, s.mileage as Mileage, s.to_do_list as TODO_List, s.resource as Resources, s.check_list as Check_List, s.travelling_mode as Travelling_Mode, s.accommodation as Accomodation, s.meals as Meals " +
                                                            "from schedule s, project p, visit_type vt " +
                                                            "where (s.proj_id = p.proj_id) and (s.visit_type_id = vt.visit_type_id) " +
                                                            " order by s.sch_no, s.proj_id;");

            dt.Load(reader);
        

            return dt;
        }


        //for autocomplete project names
        private AutoCompleteStringCollection projectNameAutoComplete()
        {
            DataTable dt = new DataTable();

            MySqlDataReader reader = DBConnection.getData("select proj_name from project");
            dt.Load(reader);

            AutoCompleteStringCollection colString = new AutoCompleteStringCollection();

            foreach (DataRow item in dt.Rows)
            {
                colString.Add(Convert.ToString(item[0]));
            }

            return colString;
        }

        //for autocomplete client names
        private AutoCompleteStringCollection clientNameAutoComplete()
        {
            DataTable dt = new DataTable();

            MySqlDataReader reader = DBConnection.getData("select name from client");
            dt.Load(reader);

            AutoCompleteStringCollection colString = new AutoCompleteStringCollection();

            foreach (DataRow item in dt.Rows)
            {
                colString.Add(Convert.ToString(item[0]));
            }

            return colString;
        }
    }
}
