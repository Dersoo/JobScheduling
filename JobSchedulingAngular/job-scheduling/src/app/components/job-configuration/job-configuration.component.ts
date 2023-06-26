import { Component, OnInit  } from '@angular/core';
import { Observable } from 'rxjs';
import { JobsService } from '../../services/jobs.service';
import { IConfiguration } from '../../models/configuration';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import { MatListOption } from '@angular/material/list';

@Component({
  selector: 'app-job-configuration',
  templateUrl: './job-configuration.component.html',
  styleUrls: ['./job-configuration.component.css']
})
export class JobConfigurationComponent  implements OnInit {

  unitsOfTime: string[] = ["m", "h"];
  daysOfTheWeek: string[] = ['MON', 
                            'TUE', 
                            'WED', 
                            'THU', 
                            'FRI',
                            'SAT',
                            'SUN'];
  selectedDaysOfTheWeek: string[] = [];

  jobConfiguration: IConfiguration = {
    unitOfTime: "",
    unitOfTimeValue: 0,
    hours: "",
    daysOfTheWeek: [],
    isActive: false
  };

  configurationForm !: FormGroup;
  actionBtn : string = "Save";

  constructor(private jobsService: JobsService, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.configurationForm = this.formBuilder.group({
      unitOfTime : ['', Validators.nullValidator],
      unitOfTimeValue : ['', Validators.nullValidator],
      hours : ['', Validators.nullValidator],
      daysOfTheWeek : ['', Validators.nullValidator],
      isActive : ['', Validators.nullValidator]
    })

    this.jobsService.getConfiguration().subscribe(data => {
      this.jobConfiguration = data;
      console.log(this.jobConfiguration);

      this.configurationForm.controls['unitOfTime'].setValue(this.jobConfiguration.unitOfTime);
      this.configurationForm.controls['unitOfTimeValue'].setValue(this.jobConfiguration.unitOfTimeValue);
      this.configurationForm.controls['hours'].setValue(this.jobConfiguration.hours);
      this.configurationForm.controls['daysOfTheWeek'].setValue(this.jobConfiguration.daysOfTheWeek);
      this.configurationForm.controls['isActive'].setValue(this.jobConfiguration.isActive);
    });
  }

  saveConfiguration()
  {
    this.jobConfiguration = this.configurationForm.value as IConfiguration;
    this.jobConfiguration.daysOfTheWeek = this.selectedDaysOfTheWeek;

    this.jobsService.setConfiguration(this.jobConfiguration)
    .subscribe({
      next: () => {
        alert("Configuration successfully updated!");
      },
      error: () => {
        alert("Error while updating the configuration!");
      }
    })
    
    console.log(this.jobConfiguration);
  }

  isChacked(day: string)
  {
    return this.jobConfiguration.daysOfTheWeek.includes(day);
  }

  onGroupsChange(options: MatListOption[]) {
    this.selectedDaysOfTheWeek = options.map(o => o.value);
    console.log(this.selectedDaysOfTheWeek);
  }
}
