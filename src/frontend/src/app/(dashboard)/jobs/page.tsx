// src/frontend/src/app/(dashboard)/jobs/page.tsx
'use client'

import { Button } from "@/components/ui/button"
import { JobList } from "@/components/jobs/job-list"
import Link from "next/link"

export default function JobsPage() {
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold">Jobs</h1>
        <Button asChild>
          <Link href="/jobs/new">Neuer Job</Link>
        </Button>
      </div>
      <JobList />
    </div>
  )
}
