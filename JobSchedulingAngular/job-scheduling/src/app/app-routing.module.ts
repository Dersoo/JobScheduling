import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JobConfigurationComponent } from './components/job-configuration/job-configuration.component';

const routes: Routes = [
  { path: '', component: JobConfigurationComponent },
  { path: 'job-configuration', component: JobConfigurationComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
